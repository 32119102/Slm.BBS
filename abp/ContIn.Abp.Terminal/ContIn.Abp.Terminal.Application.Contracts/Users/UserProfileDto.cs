namespace ContIn.Abp.Terminal.Application.Contracts.Users
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserProfileDto : UserSimpleDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 是否设置密码
        /// </summary>
        public bool PasswordSet { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// 邮箱是否验证
        /// </summary>
        public int EmailVerified { get; set; }
        /// <summary>
        /// 个人中心背景图片
        /// </summary>
        public string? BackgroundImage { get; set; }
        /// <summary>
        /// 个人主页
        /// </summary>
        public string? HomePage { get; set; }
    }
}
