namespace ContIn.Abp.Terminal.Domain.Shared.Options
{
    /// <summary>
    /// Jwt配置
    /// </summary>
    public class JwtOptions
    {
        public const string Position = "Jwt";

        /// <summary>
        /// 域
        /// </summary>
        public string Domain { get; set; } = string.Empty;
        /// <summary>
        /// 密钥
        /// </summary>
        public string SecurityKey { get; set; } = string.Empty;

        /// <summary>
        /// 过期时间
        /// </summary>
        public int Expires { get; set; }
    }
}
