using ContIn.Abp.Terminal.FreeRedis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;

namespace ContIn.Abp.Terminal.Caching.FreeRedis
{
    [DependsOn(
        typeof(AbpCachingModule),
        typeof(TerminalFreeRedisModule)
        )]
    public class TerminalCachingFreeRedisModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // 缓存配置
            Configure<AbpDistributedCacheOptions>(options =>
            {
                // 隐藏从缓存服务器写入/读取值时的错误
                options.HideErrors = true;
                // 如果你的缓存服务器由多个应用程序共同使用, 则可以为应用程序的缓存键设置一个前缀. 在这种情况下, 不同的应用程序不能覆盖彼此的缓存内容
                options.KeyPrefix = "bbs:";
                // 设置缓存时效默认值
                options.GlobalCacheEntryOptions.SlidingExpiration = TimeSpan.FromDays(7);
            });

            Configure<FreeRedisCacheOptions>(context.Services.GetConfiguration().GetSection(FreeRedisCacheOptions.Position));

            context.Services.Replace(ServiceDescriptor.Singleton<IDistributedCache, TerminalFreeRedisCache>());
        }
    }
}
