namespace ContIn.Abp.Terminal.HttpApi.Host.RedisConfiguration
{
    public static class RedisConfigurationExtensions
    {
        public static void AddRedisConfiguration(this IConfigurationBuilder builder, Action<RedisConfigurationOptions> options)
        {
            builder.Add(new RedisConfigurationSource(options));
        }
        public static void AddRedisConfiguration(this IHostBuilder hostBuilder, Action<RedisConfigurationOptions> options)
        {
            hostBuilder.ConfigureAppConfiguration((host, builder) =>
            {
                builder.AddRedisConfiguration(options);
            });
        }
    }
}
