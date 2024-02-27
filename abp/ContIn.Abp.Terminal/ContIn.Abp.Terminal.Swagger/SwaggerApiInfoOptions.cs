namespace ContIn.Abp.Terminal.Swagger
{
    /// <summary>
    /// swagger 配置
    /// </summary>
    public class SwaggerApiInfoOptions
    {
        // 节点
        public const string Position = "Swagger";

        /// <summary>
        /// 版本号
        /// </summary>
        public string? ApiVersion { get; set; }
        /// <summary>
        /// 名称前缀
        /// </summary>
        public string? NamePrefix { get; set; }
        /// <summary>
        /// 分组信息
        /// </summary>
        public List<SwaggerApiDocInfo> ApiDocs { get; set; } = new List<SwaggerApiDocInfo>();
    }
    /// <summary>
    /// swagger 分组
    /// </summary>
    public class SwaggerApiDocInfo
    {
        /// <summary>
        /// 前缀
        /// </summary>
        public string? UrlPrefix { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }
    }
}
