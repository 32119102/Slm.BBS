namespace ContIn.Abp.Terminal.Domain.Shared.Options
{
    public class AppOptions
    {
        public const string Position = "App";

        /// <summary>
        /// 应用ID
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 跨域设置
        /// </summary>
        public string? CorsOrigins { get; set; }
        /// <summary>
        /// 接口基地址
        /// </summary>
        public string? Domain { get; set; }
        /// <summary>
        /// 社区基地址
        /// </summary>
        public string? SiteBaseUrl { get; set; }
    }
}
