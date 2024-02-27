using ContIn.Abp.Terminal.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace ContIn.Abp.Terminal.HttpApi.Client
{
    [DependsOn(
        typeof(AbpHttpClientModule),
        typeof(TerminalApplicationContractsModule)
        )]
    public class TerminalHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // 创建动态客户端代码
            context.Services.AddHttpClientProxies(
                typeof(TerminalApplicationContractsModule).Assembly,
                remoteServiceConfigurationName: "Default"
                );
        }
    }
}
