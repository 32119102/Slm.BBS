namespace ContIn.Abp.Terminal.HttpApi.Host.RedisConfiguration
{
    /// <summary>
    /// 配置参数
    /// </summary>
    public class RedisConfigurationOptions
    {
        /// <summary>
        /// 连接地址
        /// </summary>
        public string? ConnectionString { get; set; }
        /// <summary>
        /// 缓存键
        /// </summary>
        public string? ConfigKey { get; set; }

        /// <summary>
        /// 重新加在配置间隔，秒
        /// </summary>
        public int ReloadSeconds { get; set; } = 10;
    }
}
