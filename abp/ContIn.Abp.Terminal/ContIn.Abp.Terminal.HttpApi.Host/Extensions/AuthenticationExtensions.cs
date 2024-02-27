using ContIn.Abp.Terminal.Domain.Shared.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ContIn.Abp.Terminal.HttpApi.Host
{
    /// <summary>
    /// 授权认证扩展服务
    /// </summary>
    public static class AuthenticationExtensions
    {
        /// <summary>
        /// 配置JWT
        /// </summary>
        /// <param name="services"></param>
        public static void AddJwtAuthentication(this IServiceCollection services)
        {
            // 获取预配置项
            var jwtOptions = services.ExecutePreConfiguredActions<JwtOptions>();
            // 身份验证
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidateIssuer = true,
                           ValidateAudience = true,
                           ValidateLifetime = true,
                           ClockSkew = TimeSpan.FromSeconds(30),
                           ValidateIssuerSigningKey = true,
                           ValidAudience = jwtOptions?.Domain,
                           ValidIssuer = jwtOptions?.Domain,
                           IssuerSigningKey = new SymmetricSecurityKey(jwtOptions?.SecurityKey.GetBytes())
                       };
                   });
        }
    }
}
