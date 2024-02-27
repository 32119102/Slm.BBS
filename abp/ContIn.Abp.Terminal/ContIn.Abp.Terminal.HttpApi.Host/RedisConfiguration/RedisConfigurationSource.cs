namespace ContIn.Abp.Terminal.HttpApi.Host.RedisConfiguration
{
    /// <summary>
    /// redis配置数据源
    /// </summary>
    public class RedisConfigurationSource : IConfigurationSource
    {
        private readonly Action<RedisConfigurationOptions> _options;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"></param>
        public RedisConfigurationSource(Action<RedisConfigurationOptions> options)
        {
            _options = options;
        }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new RedisConfigurationProvider(_options);
        }
    }
}
