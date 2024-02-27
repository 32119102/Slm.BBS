using ContIn.Abp.Terminal.Application.Contracts;
using ContIn.Abp.Terminal.Domain;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AutoMapper;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;

namespace ContIn.Abp.Terminal.Application
{
    /// <summary>
    /// 应用服务
    /// </summary>
    [DependsOn(
        typeof(TerminalDomainModule),
        typeof(TerminalApplicationContractsModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule)
        )]
    public class TerminalApplicationModule : AbpModule
    {
        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="context"></param>
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                // Add all mappings defined in the assembly of the MyModule class
                //options.AddMaps<TerminalApplicationModule>();
                // 
                options.AddProfile<TerminalApplicationAutoMapperProfile>();
                options.AddProfile<UserAutoMapperProfile>();
            });

            Configure<AbpSequentialGuidGeneratorOptions>(options =>
            {
                /*
                 SequentialAtEnd (default) 用于SQL Server.
                 SequentialAsString 用于MySQL和PostgreSQL.
                 SequentialAsBinary 用于Oracle.
                 */
                options.DefaultSequentialGuidType = SequentialGuidType.SequentialAsString;
            });
        }
    }
}
