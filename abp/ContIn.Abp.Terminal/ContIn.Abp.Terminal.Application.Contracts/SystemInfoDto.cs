namespace ContIn.Abp.Terminal.Application.Contracts
{
    /// <summary>
    /// 服务器信息
    /// </summary>
    public class SystemInfoDto
    {
        /// <summary>
        /// 系统版本
        /// </summary>
        public string? OsVersion { get; set; }
        /// <summary>
        /// 系统架构
        /// </summary>
        public string? Arch { get; set; }
        /// <summary>
        /// 线程数量
        /// </summary>
        public string? ProcessorCount { get; set; }
        /// <summary>
        /// 程序物理路径
        /// </summary>
        public string? ApplicationBasePath { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        public string? ApplicationName { get; set; }
        /// <summary>
        /// 应用版本
        /// </summary>
        public string? ApplicationVersion { get; set; }
        /// <summary>
        /// 框架版本
        /// </summary>
        public string? RuntimeFramework { get; set; }
        /// <summary>
        /// 服务器IP
        /// </summary>
        public string? Host { get; set; }
        /// <summary>
        /// 服务器端口
        /// </summary>
        public string? Port { get; set; }
        /// <summary>
        /// 服务器名称
        /// </summary>
        public string? MachineName { get; set; }
    }
}
