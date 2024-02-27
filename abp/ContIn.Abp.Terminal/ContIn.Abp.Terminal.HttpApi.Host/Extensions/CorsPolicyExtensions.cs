using Microsoft.AspNetCore.Cors;

namespace ContIn.Abp.Terminal.HttpApi.Host
{
    /// <summary>
    /// 跨域配置
    /// </summary>
    public static class CorsPolicyExtensions
    {
        /// <summary>
        /// 配置跨域
        /// </summary>
        /// <param name="services"></param>
        public static void AddCorsPolicy(this IServiceCollection services)
        {
            var corsOrigins = services.GetConfiguration()["App:CorsOrigins"];
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder
                        .WithOrigins(
                            corsOrigins.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(o => o.RemovePostFix("/")).ToArray()
                        )
                        .WithAbpExposedHeaders()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }
    }
}
