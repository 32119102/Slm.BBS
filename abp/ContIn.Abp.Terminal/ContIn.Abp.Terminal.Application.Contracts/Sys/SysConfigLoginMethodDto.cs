namespace ContIn.Abp.Terminal.Application.Contracts.Sys
{
    /// <summary>
    /// 登录方式
    /// </summary>
    public class SysConfigLoginMethodDto
    {
        /// <summary>
        /// 密码登录
        /// </summary>
        public bool Password { get; set; }
        /// <summary>
        /// QQ登录
        /// </summary>
        public bool Qq { get; set; }
        /// <summary>
        /// GitHub登录
        /// </summary>
        public bool Github { get; set; }
        /// <summary>
        /// OSChina登录
        /// </summary>
        public bool Osc { get; set; }
    }
}
