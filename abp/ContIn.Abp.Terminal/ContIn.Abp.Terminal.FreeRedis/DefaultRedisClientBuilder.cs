using FreeRedis;
using Volo.Abp.DependencyInjection;

namespace ContIn.Abp.Terminal.FreeRedis
{
    public class DefaultRedisClientBuilder : IRedisClientBuilder, ITransientDependency
    {
        public virtual RedisClient CreateClient(FreeRedisConfiguration configuration)
        {
            if (configuration.Mode == RedisModeEnum.Sentinel)
            {
                return new RedisClient(configuration.ConnectionString, configuration.Sentinels.ToArray(), configuration.ReadOnly);
            }
            else
            {
                return new RedisClient(configuration.ConnectionString);
            }
        }
    }
}
