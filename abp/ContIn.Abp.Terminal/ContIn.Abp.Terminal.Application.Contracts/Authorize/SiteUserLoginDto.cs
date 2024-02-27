using ContIn.Abp.Terminal.Application.Contracts.Users;

namespace ContIn.Abp.Terminal.Application.Contracts.Authorize
{
    /// <summary>
    /// 登录响应信息
    /// </summary>
    public class SiteUserLoginDto
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserDto? User { get; set; }
        /// <summary>
        /// token
        /// </summary>
        public string? Token { get; set; }
    }
}
