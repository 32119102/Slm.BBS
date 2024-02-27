using ContIn.Abp.Terminal.Application;
using ContIn.Abp.Terminal.Caching.FreeRedis;
using ContIn.Abp.Terminal.Domain.Shared.Options;
using ContIn.Abp.Terminal.EntityFrameworkCore;
using ContIn.Abp.Terminal.Swagger;
using Serilog;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.BlobStoring.Minio;
using Volo.Abp.Modularity;

namespace ContIn.Abp.Terminal.HttpApi.Host
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpBlobStoringFileSystemModule),
        typeof(AbpBlobStoringMinioModule),
        typeof(TerminalEntityFrameworkCoreModule),
        typeof(TerminalApplicationModule),
        typeof(TerminalHttpApiModule),
        typeof(TerminalCachingFreeRedisModule),
        typeof(TerminalSwaggerModule)
        )]
    public class TerminalHttpApiHostModule : AbpModule
    {
        /// <summary>
        /// 预配置
        /// </summary>
        /// <param name="context"></param>
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            PreConfigure<JwtOptions>(options =>
            {
                options.Domain = configuration.GetValue<string>("Jwt:Domain");
                // options.Expires = configuration.GetValue<int>("Jwt:Expires");
                options.SecurityKey = configuration.GetValue<string>("Jwt:SecurityKey");
            });
        }
        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="context"></param>
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // 身份验证
            context.Services.AddJwtAuthentication();
            // 认证授权
            context.Services.AddAuthorization();
            // 注册 IHttpClientFactory
            context.Services.AddHttpClient();
            // 配置服务为API控制器
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options
                    .ConventionalControllers
                    .Create(typeof(TerminalApplicationModule).Assembly, opts =>
                    {
                        // 默认 app
                        opts.RootPath = "app";
                    });
            });
            // 设置跨域
            context.Services.AddCorsPolicy();
            // BLOB存储
            context.Services.AddBlobContainers();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();

            // 身份验证
            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("CorsPolicy");
            // 认证授权
            app.UseAuthorization();

            app.UseConfiguredEndpoints();
        }
    }
}
