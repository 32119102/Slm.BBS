namespace ContIn.Abp.Terminal.Application.Contracts.Authorize
{
    /// <summary>
    /// github授权
    /// </summary>
    public interface IAuthorizeService
    {
        /// <summary>
        /// 获取github登录地址
        /// </summary>
        /// <returns></returns>
        Task<string> GetLoginAddressAsync();
        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<string> GetAccessTokenAsync(string code);
        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        Task<string> GenerateTokenAsync(string access_token);
        /// <summary>
        /// 用户名密码登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<string> SignInAsync(UserPwdLoginDto input);
        /// <summary>
        /// 社区用户名密码登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<SiteUserLoginDto> SiteSignInAsync(UserPwdLoginDto input);
        /// <summary>
        /// 社区用户退出登录
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        Task SiteSignoutAsync(string source);
        /// <summary>
        /// 刷新JwtToken
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<string> RefreshTokenAsync(RefreshTokenInputDto input);

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<SiteUserLoginDto> SiteSignUpAsync(SiteUserSignupDto input);
    }
}
