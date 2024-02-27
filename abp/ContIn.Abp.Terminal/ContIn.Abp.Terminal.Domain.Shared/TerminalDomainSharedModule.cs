using ContIn.Abp.Terminal.Domain.Shared.Exceptions;
using ContIn.Abp.Terminal.Domain.Shared.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Volo.Abp.Modularity;

namespace ContIn.Abp.Terminal.Domain.Shared
{
    public class TerminalDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // 注册自定义异常处理
            Configure<MvcOptions>(options =>
            {
                var filterItem = options.Filters
                                        .Where(f => (f is ServiceFilterAttribute))
                                        .Cast<ServiceFilterAttribute>()
                                        .FirstOrDefault(f => f.ServiceType.Name.Equals("AbpExceptionFilter"));
                if (filterItem != null)
                    options.Filters.Remove(filterItem);
                //添加自己的异常过滤器
                options.Filters.AddService(typeof(GlobalExceptionFilter));
            });

            ConfigureConfigOptions(context.Services.GetConfiguration());
        }

        private void ConfigureConfigOptions(IConfiguration configuration)
        {
            Configure<GitHubOptions>(configuration.GetSection(GitHubOptions.Position));
            Configure<JwtOptions>(configuration.GetSection(JwtOptions.Position));
            Configure<AppOptions>(configuration.GetSection(AppOptions.Position));
        }
    }
}
