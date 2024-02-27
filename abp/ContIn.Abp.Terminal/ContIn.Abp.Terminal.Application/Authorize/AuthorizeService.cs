using ContIn.Abp.Terminal.Application.Contracts.Authorize;
using ContIn.Abp.Terminal.Application.Contracts.Captcha;
using ContIn.Abp.Terminal.Application.Contracts.Users;
using ContIn.Abp.Terminal.Domain.Shared;
using ContIn.Abp.Terminal.Domain.Shared.Enum;
using ContIn.Abp.Terminal.Domain.Shared.Options;
using ContIn.Abp.Terminal.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using Volo.Abp.Application.Services;
using Volo.Abp.Json;
using Volo.Abp.Security.Claims;

namespace ContIn.Abp.Terminal.Application.Authorize
{
    /// <summary>
    /// Github授权
    /// </summary>
    [ApiExplorerSettings(GroupName = SwaggerGroupName.Auth)]
    public class AuthorizeService : ApplicationService, IAuthorizeService
    {
        // github配置
        private readonly GitHubOptions _gitHubOptions;
        // 授权范围
        private readonly string _scope = "user";
        // HttpClientFactory
        private readonly IHttpClientFactory _httpClientFactory;
        // json序列化
        private readonly IJsonSerializer _jsonSerializer;
        // Jwt配置
        private readonly JwtOptions _jwtOptions;
        // 验证码fuw
        private readonly ICaptchaAppService _captchaService;
        // 用户仓储接口
        private readonly IUserRepository _userRepository;
        // 缓存服务
        private readonly IDistributedCache _cache;
        // jwttoken缓存前缀
        private readonly string _jwtTokenCachePrefix = "c:bbs:JwtToken_";
        private readonly AppOptions _appOptions;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gitHubOptions"></param>
        /// <param name="httpClientFactory"></param>
        /// <param name="jsonSerializer"></param>
        /// <param name="jwtOptions"></param>
        /// <param name="captchaAppService"></param>
        /// <param name="userRepository"></param>
        /// <param name="cache"></param>
        /// <param name="appOptions"></param>
        public AuthorizeService(IOptionsMonitor<GitHubOptions> gitHubOptions, IHttpClientFactory httpClientFactory, IJsonSerializer jsonSerializer, 
            IOptionsMonitor<JwtOptions> jwtOptions, ICaptchaAppService captchaAppService, IUserRepository userRepository, IDistributedCache cache, IOptionsMonitor<AppOptions> appOptions)
        {
            _gitHubOptions = gitHubOptions.CurrentValue;
            _httpClientFactory = httpClientFactory;
            _jsonSerializer = jsonSerializer;
            _jwtOptions = jwtOptions.CurrentValue;
            _captchaService = captchaAppService;
            _userRepository = userRepository;
            _cache = cache;
            _appOptions = appOptions.CurrentValue;
        }

        /// <summary>
        /// 生成JwtToken
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<string> GenerateTokenAsync(string access_token)
        {
            if (access_token.IsNullOrWhiteSpace())
            {
                throw new CustomException(TerminalErrorCodes.AuthorizeAccessTokenIsEmpty, "授权AccessToken为空");
            }
            using var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.14 Safari/537.36 Edg/83.0.478.13");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"token {access_token}");
            var httpResponse = await httpClient.GetAsync($"{_gitHubOptions.ApiUserInfoUrl}");
            var content = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new CustomException(TerminalErrorCodes.AuthorizeGetUserInfoException, content);
            }
            Logger.LogInformation("获取github用户信息：" + content);

            var user = _jsonSerializer.Deserialize<GitHubUserInfoDto>(content);

            var token = await GenerateJwtTokenAsync(user.id.ToString(), user.name, user.email);
            Logger.LogInformation("Github.JwtToken:" + user.id + "," + token);
            return token;
        }

        /// <summary>
        /// 生成JwtToken
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="source">来源平台</param>
        /// <returns></returns>
        private async Task<string> GenerateJwtTokenAsync(string id, string name, string email, string source = "admin")
        {
            var nowTime = Clock.Now;
            var claims = new[] {
                    new Claim(AbpClaimTypes.UserId, id),
                    new Claim(AbpClaimTypes.UserName, name),
                    new Claim(AbpClaimTypes.Email, email),
                    new Claim(JwtRegisteredClaimNames.Exp, $"{nowTime.AddMinutes(_jwtOptions.Expires).ToSecondsTimestamp()}"),
                    new Claim(JwtRegisteredClaimNames.Nbf, $"{nowTime.ToSecondsTimestamp()}")
                };

            var key = new SymmetricSecurityKey(_jwtOptions.SecurityKey.GetBytes());
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(
                issuer: _jwtOptions.Domain,
                audience: _jwtOptions.Domain,
                claims: claims,
                expires: nowTime.AddMinutes(_jwtOptions.Expires),
                signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            await _cache.SetStringAsync(GenerateJwtTokenCacheKey(source, id), token, new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = Clock.Now.AddMinutes(_jwtOptions.Expires * 2)
            });
            return token;
        }
        /// <summary>
        /// jwttoken 缓存键
        /// </summary>
        /// <param name="source"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GenerateJwtTokenCacheKey(string source, string id) => $"{_jwtTokenCachePrefix}:{_appOptions.Id}:{source}:{id}";

        /// <summary>
        /// 根据code获取AccessToken
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task<string> GetAccessTokenAsync(string code)
        {
            if (code.IsNullOrWhiteSpace())
            {
                throw new CustomException(TerminalErrorCodes.AuthorizeCodeIsEmpty, "授权code为空");
            }
            var content = new StringContent(
                $"code={code}&client_id={_gitHubOptions.ClientID}&redirect_uri={_gitHubOptions.RedirectUri}&client_secret={_gitHubOptions.ClientSecret}");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            using var client = _httpClientFactory.CreateClient();
            var httpResponse = await client.PostAsync(_gitHubOptions.ApiAccessTokenUrl, content);
            var response = await httpResponse.Content.ReadAsStringAsync();

            // error
            // error=incorrect_client_credentials&error_description=The+client_id+and/or+client_secret+passed+are+incorrect.
            // &error_uri=https://docs.github.com/apps/managing-oauth-apps/troubleshooting-oauth-app-access-token-request-errors/#incorrect-client-credentials
            // success
            // access_token=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&scope=public_repo%2Cuser&token_type=bearer

            if (!response.StartsWith("access_token"))
            {
                throw new CustomException(TerminalErrorCodes.AuthorizeAccessTokenException, response);
            }

            return response.Split('&')[0].Split('=')[1];
        }

        /// <summary>
        /// 获取github登录地址
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetLoginAddressAsync()
        {
            var address = string.Concat(new string[]
            {
                    _gitHubOptions.ApiAuthorizeUrl,
                    "?client_id=", _gitHubOptions.ClientID,
                    "&scope=", _scope,
                    "&state=", GuidGenerator.Create().ToString("N"),
                    "&redirect_uri=", _gitHubOptions.RedirectUri
            });
            return await Task.FromResult(address);
        }

        /// <summary>
        /// 用户名密码登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<string> SignInAsync(UserPwdLoginDto input)
        {
            // 验证码
            await _captchaService.ValideCaptchaCodeAsync(input.CaptchaId, input.CaptchaCode);
            // 用户信息
            var user = await _userRepository.GetUserByNameOrEmailAsync(input.UserName!);
            if (user == null || user.Status != StatusEnum.Normal)
            {
                throw new CustomException(TerminalErrorCodes.AuthorizeUserNotExistsOrForbidden, "用户不存在或被禁用");
            }
            if (!user.Password!.Equals(input.Password, StringComparison.OrdinalIgnoreCase))
            {
                throw new CustomException(TerminalErrorCodes.AuthorizeUserPwdFail, "密码错误");
            }
            // 只有 owner 和 admin 能登录
            if (user.Roles.IsNullOrWhiteSpace() || (
                !(user.Roles ?? string.Empty).Contains(RoleTypeEnum.Owner.ToString().ToLower()) && !(user.Roles ?? string.Empty).Contains(RoleTypeEnum.Admin.ToString().ToLower()))
            )
            {
                throw new CustomException(TerminalErrorCodes.AuthorizeUserNotExistsOrForbidden, "此用户不允许登录后台管理系统");
            }
            // JwtToken
            var token = await GenerateJwtTokenAsync(user.Id.ToString(), user.UserName!, user.Email!);
            Logger.LogInformation("SignIn.JwtToken:" + user.Id + "," + token);
            return token;
        }

        /// <summary>
        /// 社区用户名密码登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task<SiteUserLoginDto> SiteSignInAsync(UserPwdLoginDto input)
        {
            // 验证码
            await _captchaService.ValideCaptchaCodeAsync(input.CaptchaId, input.CaptchaCode);
            // 用户信息
            var user = await _userRepository.GetUserByNameOrEmailAsync(input.UserName!);
            if (user == null || user.Status != StatusEnum.Normal)
            {
                throw new CustomException(TerminalErrorCodes.AuthorizeUserNotExistsOrForbidden, "用户不存在或被禁用");
            }
            if (!user.Password!.Equals(input.Password, StringComparison.OrdinalIgnoreCase))
            {
                throw new CustomException(TerminalErrorCodes.AuthorizeUserPwdFail, "密码错误");
            }
            // JwtToken
            var token = await GenerateJwtTokenAsync(user.Id.ToString(), user.UserName!, user.Email!, "site");
            Logger.LogInformation("SiteSignIn.JwtToken:" + user.Id + "," + token);
            return new SiteUserLoginDto { User = ObjectMapper.Map<User, UserDto>(user), Token = token };
        }

        /// <summary>
        /// 刷新JwtToken
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<string> RefreshTokenAsync(RefreshTokenInputDto input)
        {
            var payload = input.Token!.Split(".")[1];
            var claims = _jsonSerializer.Deserialize<Dictionary<string, string>>(Base64UrlEncoder.Decode(payload));

            claims.TryGetValue(AbpClaimTypes.UserId, out string? id);
            claims.TryGetValue(AbpClaimTypes.UserName, out string? username);
            claims.TryGetValue(AbpClaimTypes.Email, out string? email);

            if (id.IsNullOrWhiteSpace())
            {
                throw new CustomException(TerminalErrorCodes.AuthorizeJwtTokenInvalid, "登录已过期，请重新登录");
            }

            var c = await _cache.GetStringAsync($"{_jwtTokenCachePrefix}:{_appOptions.Id}:{id}");
            if (c.IsNullOrWhiteSpace() || !c.Equals(input.Token))
            {
                throw new CustomException(TerminalErrorCodes.AuthorizeJwtTokenInvalid, "登录已过期，请重新登录");
            }

            var newToken = await GenerateJwtTokenAsync(id!, username ?? string.Empty, email ?? string.Empty);
            Logger.LogInformation("Refresh.JwtToken:" + id + "," + newToken);
            return newToken;
        }

        /// <summary>
        /// 社区用户退出登录
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [Authorize]
        public async Task SiteSignoutAsync(string source)
        {
            var userId = CurrentUser.GetUserId();
            await _cache.RemoveAsync(GenerateJwtTokenCacheKey(source, userId.ToString()));
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        [AllowAnonymous]
        public async Task<SiteUserLoginDto> SiteSignUpAsync(SiteUserSignupDto input)
        {
            // 验证码
            await _captchaService.ValideCaptchaCodeAsync(input.CaptchaId, input.CaptchaCode);
            // 用户名是否占用
            var user = await _userRepository.GetUserByUserNameAsync(input.Username!);
            if (user != null)
            {
                throw new CustomException(TerminalErrorCodes.CommonErrorCode, "用户名：" + input.Username + " 已被占用");
            }
            // 邮箱是否占用
            user = await _userRepository.GetUserByEmailAsync(input.Email!);
            if (user != null)
            {
                throw new CustomException(TerminalErrorCodes.CommonErrorCode, "邮箱：" + input.Email + " 已被占用");
            }
            user = await _userRepository.InsertAsync(new User()
            {
                UserName = input.Username,
                Email = input.Email,
                Nickname = input.Nickname,
                Password = input.Password,
                Status = StatusEnum.Normal,
                CreateTime = Clock.Now.ToMillisecondsTimestamp(),
                UpdateTime = Clock.Now.ToMillisecondsTimestamp(),
                Roles = RoleTypeEnum.User.ToString().ToLower()
            }, true);
            var token = await GenerateJwtTokenAsync(user.Id.ToString(), user.UserName!, user.Email!, "site");
            return new SiteUserLoginDto { User = ObjectMapper.Map<User, UserDto>(user), Token = token };
        }
    }
}
