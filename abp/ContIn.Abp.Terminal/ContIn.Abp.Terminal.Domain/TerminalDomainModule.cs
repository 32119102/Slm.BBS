using ContIn.Abp.Terminal.Domain.Shared;
using Volo.Abp.Modularity;

namespace ContIn.Abp.Terminal.Domain
{
    [DependsOn(typeof(TerminalDomainSharedModule))]
    public class TerminalDomainModule : AbpModule
    {

    }
}
