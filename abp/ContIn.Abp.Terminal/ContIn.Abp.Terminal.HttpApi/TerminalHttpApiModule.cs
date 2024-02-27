using ContIn.Abp.Terminal.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.Json.Newtonsoft;
using Volo.Abp.Modularity;

namespace ContIn.Abp.Terminal.HttpApi
{
    /// <summary>
    /// HttpApi
    /// </summary>
    [DependsOn(
        typeof(TerminalApplicationContractsModule),
        typeof(AbpJsonModule)
        )]
    public class TerminalHttpApiModule : AbpModule
    {
        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="context"></param>
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var preActions = context.Services.GetPreConfigureActions<AbpJsonOptions>();
            Configure<AbpJsonOptions>(options =>
            {
                options.Providers.Add<AbpNewtonsoftJsonSerializerProvider>();
            });

            Configure<AbpNewtonsoftJsonSerializerOptions>(options =>
            {
                options.Converters.Add<AbpJsonIsoDateTimeConverter>();
            });
            
        }
    }
}
