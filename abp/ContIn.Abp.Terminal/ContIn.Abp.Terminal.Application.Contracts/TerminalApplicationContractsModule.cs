using ContIn.Abp.Terminal.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;

namespace ContIn.Abp.Terminal.Application.Contracts
{
    /// <summary>
    /// 应用服务
    /// </summary>
    [DependsOn(
        typeof(TerminalDomainSharedModule),
        typeof(AbpFluentValidationModule)
        )]
    public class TerminalApplicationContractsModule : AbpModule
    {
        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="context"></param>
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            
        }
    }
}
