using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace ContIn.Abp.Terminal.Swagger
{
    public class TerminalSwaggerModule : AbpModule
    {
        private readonly SwaggerApiInfoOptions _swaggerOptions = new();
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var service = context.Services;

            service.GetConfiguration().GetSection(SwaggerApiInfoOptions.Position).Bind(_swaggerOptions);

            service.AddSwaggerGen(c =>
            {
                c.DocumentFilter<SwaggerDocumentFilter>();
                // c.OperationFilter<OptionalRouteParameterOperationFilter>();

                foreach (var doc in _swaggerOptions.ApiDocs)
                {
                    c.SwaggerDoc(doc.UrlPrefix, new OpenApiInfo()
                    {
                        Version = _swaggerOptions.ApiVersion,
                        Title = _swaggerOptions.NamePrefix + doc.Name,
                        Description = doc.Description
                    });
                }

                Directory.GetFiles(AppContext.BaseDirectory, "*.xml").ToList().ForEach(xmlFile =>
                {
                    c.IncludeXmlComments(xmlFile);
                });

                var security = new OpenApiSecurityScheme
                {
                    Description = "JWT模式授权，请输入 Bearer {Token} 进行身份验证",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                };
                c.AddSecurityDefinition("oauth2", security);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement { { security, new List<string>() } });
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                _swaggerOptions.ApiDocs.ForEach(doc =>
                {
                    options.SwaggerEndpoint($"/swagger/{doc.UrlPrefix}/swagger.json", doc.Name);
                });
                // 模型的默认扩展深度，设置为 -1 完全隐藏模型
                options.DefaultModelsExpandDepth(-1);
                // API文档仅展开标记
                options.DocExpansion(DocExpansion.List);
                // API前缀设置为空
                options.RoutePrefix = string.Empty;
                // API页面Title
                options.DocumentTitle = _swaggerOptions.NamePrefix + "接口文档";
            });
        }
    }
}
