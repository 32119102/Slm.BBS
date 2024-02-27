using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace ContIn.Abp.Terminal.FreeRedis
{
    public class TerminalFreeRedisModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<TerminalFreeRedisOptions>(options =>
            {
                options.Configure(context.Services.GetConfiguration().GetSection(TerminalFreeRedisOptions.Position));
            });
            //new TerminalFreeRedisOptions().Configure(context.Services.GetConfiguration().GetSection(TerminalFreeRedisOptions.Position));
        }
    }
}
