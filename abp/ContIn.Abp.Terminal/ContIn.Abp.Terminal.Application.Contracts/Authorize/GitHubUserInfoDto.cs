namespace ContIn.Abp.Terminal.Application.Contracts.Authorize
{
    /// <summary>
    /// Github授权后的用户信息
    /// </summary>
    public class GitHubUserInfoDto
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public string? login { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string? avatar_url { get; set; }
        /// <summary>
        /// github地址
        /// </summary>
        public string? html_url { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string name { get; set; } = string.Empty;
        /// <summary>
        /// 公司
        /// </summary>
        public string? company { get; set; }
        /// <summary>
        /// 博客地址
        /// </summary>
        public string? blog { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string? location { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; } = string.Empty;
    }
}
