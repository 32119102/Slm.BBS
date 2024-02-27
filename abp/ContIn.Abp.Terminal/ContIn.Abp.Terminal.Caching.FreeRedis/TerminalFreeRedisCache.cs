using ContIn.Abp.Terminal.FreeRedis;
using FreeRedis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContIn.Abp.Terminal.Caching.FreeRedis
{
    /// <summary>
    /// 缓存操作
    /// </summary>
    public class TerminalFreeRedisCache : IDistributedCache
    {
        protected FreeRedisCacheOptions Options { get; }
        // redis client
        protected RedisClient RedisClient { get; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"></param>
        /// <param name="redisClientFactory"></param>
        public TerminalFreeRedisCache(IOptions<FreeRedisCacheOptions> options, IRedisClientFactory redisClientFactory)
        {
            Options = options.Value;
            RedisClient = redisClientFactory.Get(Options.Name);
        }

        // lua 脚本
        private const string SetScript = @"
                redis.call('HMSET', KEYS[1], 'absexp', ARGV[1], 'sldexp', ARGV[2], 'data', ARGV[4])
                if ARGV[3] ~= '-1' then
                  redis.call('EXPIRE', KEYS[1], ARGV[3])
                end
                return 1";
        private const string AbsoluteExpirationKey = "absexp";
        private const string SlidingExpirationKey = "sldexp";
        private const string DataKey = "data";
        private const long NotPresent = -1;

        public byte[] Get(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return GetAndRefresh(key, getData: true);
        }

        public async Task<byte[]> GetAsync(string key, CancellationToken token = default)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            token.ThrowIfCancellationRequested();

            return await GetAndRefreshAsync(key, getData: true, token: token);
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var creationTime = DateTimeOffset.Now;

            var absoluteExpiration = GetAbsoluteExpiration(creationTime, options);
            _ = RedisClient.Eval(SetScript, new string[] { key },
                new object[]
                {
                        absoluteExpiration?.Ticks ?? NotPresent,
                        options.SlidingExpiration?.TotalSeconds ?? NotPresent,
                        GetExpirationInSeconds(creationTime, absoluteExpiration, options) ?? NotPresent,
                        value
                });
        }

        public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            token.ThrowIfCancellationRequested();

            var creationTime = DateTimeOffset.Now;

            var absoluteExpiration = GetAbsoluteExpiration(creationTime, options);
            _ = RedisClient.Eval(SetScript, new string[] { key },
                new object[]
                {
                        absoluteExpiration?.Ticks ?? NotPresent,
                        options.SlidingExpiration?.TotalSeconds ?? NotPresent,
                        GetExpirationInSeconds(creationTime, absoluteExpiration, options) ?? NotPresent,
                        value
                });

            await Task.Delay(TimeSpan.FromMilliseconds(1));
        }

        public void Refresh(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            GetAndRefresh(key, getData: false);
        }

        public async Task RefreshAsync(string key, CancellationToken token = default)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            token.ThrowIfCancellationRequested();

            await GetAndRefreshAsync(key, getData: false, token: token);
        }

        private byte[] GetAndRefresh(string key, bool getData)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            object[] results;
            byte[] value = null;
            if (getData)
            {
                var ret = RedisClient.HMGet<byte[]>(key, AbsoluteExpirationKey, SlidingExpirationKey, DataKey);
                results = new object[] {
                    ret[0] == null ? null : Encoding.UTF8.GetString(ret[0]), 
                    ret[1] == null ? null : Encoding.UTF8.GetString(ret[1])
                };
                value = ret[2] ?? null;
            }
            else
            {
                results = RedisClient.HMGet(key, AbsoluteExpirationKey, SlidingExpirationKey);
            }

            if (results.Length >= 2)
            {
                MapMetadata(results, out DateTimeOffset? absExpr, out TimeSpan? sldExpr);
                Refresh(key, absExpr, sldExpr);
            }

            if (value != null)
            {
                return value;
            }

            return null;
        }

        private async Task<byte[]> GetAndRefreshAsync(string key, bool getData, CancellationToken token = default)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            token.ThrowIfCancellationRequested();

            object[] results;
            byte[] value = null;
            if (getData)
            {
                var ret = RedisClient.HMGet<byte[]>(key, AbsoluteExpirationKey, SlidingExpirationKey, DataKey);
                results = new object[] {
                    ret[0] == null ? null : Encoding.UTF8.GetString(ret[0]),
                    ret[1] == null ? null : Encoding.UTF8.GetString(ret[1]),
                    value = ret[2]
                };
            }
            else
            {
                results = RedisClient.HMGet(key, AbsoluteExpirationKey, SlidingExpirationKey);
            }

            if (results.Length >= 2)
            {
                MapMetadata(results, out DateTimeOffset? absExpr, out TimeSpan? sldExpr);
                await RefreshAsync(key, absExpr, sldExpr, token);
            }

            if (results.Length >= 3)
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// 移除指定的键，如果键不存在，则忽略之
        /// </summary>
        /// <param name="key">多个键已'|'分隔</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Remove(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            RedisClient.Del(key.Split('|'));
        }
        /// <summary>
        /// 移除指定的键，如果键不存在，则忽略之
        /// </summary>
        /// <param name="key"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task RemoveAsync(string key, CancellationToken token = default)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            token.ThrowIfCancellationRequested();
            RedisClient.Del(key.Split('|'));

            await Task.Delay(TimeSpan.FromMilliseconds(1));
        }

        private void MapMetadata(object[] results, out DateTimeOffset? absoluteExpiration, out TimeSpan? slidingExpiration)
        {
            absoluteExpiration = null;
            slidingExpiration = null;
            if (long.TryParse(results[0]?.ToString(), out var absoluteExpirationTicks) && absoluteExpirationTicks != NotPresent)
            {
                absoluteExpiration = new DateTimeOffset(absoluteExpirationTicks, TimeSpan.Zero);
            }
            if (long.TryParse(results[1]?.ToString(), out var slidingExpirationTicks) && slidingExpirationTicks != NotPresent)
            {
                slidingExpiration = TimeSpan.FromSeconds(slidingExpirationTicks);
            }
        }
        /// <summary>
        /// 刷新键过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="absExpr"></param>
        /// <param name="sldExpr"></param>
        /// <exception cref="ArgumentNullException"></exception>
        private void Refresh(string key, DateTimeOffset? absExpr, TimeSpan? sldExpr)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (sldExpr.HasValue)
            {
                TimeSpan? expr;
                if (absExpr.HasValue)
                {
                    var relExpr = absExpr.Value - DateTimeOffset.Now;
                    expr = relExpr <= sldExpr.Value ? relExpr : sldExpr;
                }
                else
                {
                    expr = sldExpr;
                }
                if (expr.HasValue)
                {
                    RedisClient.Expire(key, (TimeSpan)expr);
                }
                else
                {
                    RedisClient.Expire(key, 0);
                }
            }
        }
        /// <summary>
        /// 刷新键过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="absExpr"></param>
        /// <param name="sldExpr"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private async Task RefreshAsync(string key, DateTimeOffset? absExpr, TimeSpan? sldExpr, CancellationToken token = default)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            token.ThrowIfCancellationRequested();
            if (sldExpr.HasValue)
            {
                TimeSpan? expr;
                if (absExpr.HasValue)
                {
                    var relExpr = absExpr.Value - DateTimeOffset.Now;
                    expr = relExpr <= sldExpr.Value ? relExpr : sldExpr;
                }
                else
                {
                    expr = sldExpr;
                }
                if (expr.HasValue)
                {
                    RedisClient.Expire(key, (TimeSpan)expr);
                }
                else
                {
                    RedisClient.Expire(key, 0);
                }
            }
            await Task.Delay(TimeSpan.FromMilliseconds(1));
        }
        /// <summary>
        /// 计算键过期时间，秒
        /// </summary>
        /// <param name="creationTime"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        private static long? GetExpirationInSeconds(DateTimeOffset creationTime, DateTimeOffset? absoluteExpiration, DistributedCacheEntryOptions options)
        {
            if (absoluteExpiration.HasValue && options.SlidingExpiration.HasValue)
            {
                return (long)Math.Min(
                    (absoluteExpiration.Value - creationTime).TotalSeconds,
                    options.SlidingExpiration.Value.TotalSeconds);
            }
            else if (absoluteExpiration.HasValue)
            {
                return (long)(absoluteExpiration.Value - creationTime).TotalSeconds;
            }
            else if (options.SlidingExpiration.HasValue)
            {
                return (long)options.SlidingExpiration.Value.TotalSeconds;
            }
            return null;
        }
        /// <summary>
        /// 绝对过期时间
        /// </summary>
        /// <param name="creationTime"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private static DateTimeOffset? GetAbsoluteExpiration(DateTimeOffset creationTime, DistributedCacheEntryOptions options)
        {
            if (options.AbsoluteExpiration.HasValue && options.AbsoluteExpiration <= creationTime)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(DistributedCacheEntryOptions.AbsoluteExpiration),
                    options.AbsoluteExpiration.Value,
                    "The absolute expiration value must be in the future.");
            }
            var absoluteExpiration = options.AbsoluteExpiration;
            if (options.AbsoluteExpirationRelativeToNow.HasValue)
            {
                absoluteExpiration = creationTime + options.AbsoluteExpirationRelativeToNow;
            }

            return absoluteExpiration;
        }
    }
}
