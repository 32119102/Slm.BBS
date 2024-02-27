namespace ContIn.Abp.Terminal.Domain.Shared.Options
{
    /// <summary>
    /// GitHub配置
    /// </summary>
    public class GitHubOptions
    {
        // 节点
        public const string Position = "GitHub";

        public string UserId { get; set; } = string.Empty;
        /// <summary>
        /// 应用ID
        /// </summary>
        public string ClientID { get; set; } = string.Empty;
        /// <summary>
        /// 应用密钥
        /// </summary>
        public string ClientSecret { get; set; } = string.Empty;
        /// <summary>
        /// 回调地址
        /// </summary>
        public string RedirectUri { get; set; } = string.Empty;
        /// <summary>
        /// 应用名称
        /// </summary>
        public string ApplicationName { get; set; } = string.Empty;
        /// <summary>
        /// 跳转GitHub登录页面，获取用户授权
        /// </summary>
        public string ApiAuthorizeUrl { get; set; } = string.Empty;
        /// <summary>
        /// 根据Code获取AccessToken
        /// </summary>
        public string ApiAccessTokenUrl { get; set; } = string.Empty;
        /// <summary>
        /// 根据AccessToken获取用户信息
        /// </summary>
        public string ApiUserInfoUrl { get; set; } = string.Empty;
    }
}
