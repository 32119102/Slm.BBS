using FreeRedis;
using System.Text.Json;

namespace ContIn.Abp.Terminal.HttpApi.Host.RedisConfiguration
{
    /// <summary>
    /// redis配置提供程序
    /// </summary>
    public class RedisConfigurationProvider : ConfigurationProvider
    {
        private RedisConfigurationOptions _options;
        private readonly Task _handleRedisConfigurationTask;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"></param>
        public RedisConfigurationProvider(Action<RedisConfigurationOptions> OptionsAction)
        {
            _options = new RedisConfigurationOptions();
            OptionsAction(_options);
            
            _handleRedisConfigurationTask = new Task(HandleRedisConfurationChange);
        }

        private async void HandleRedisConfurationChange()
        {
            if (_options.ReloadSeconds <= 0)
                return;

            while (true)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(_options.ReloadSeconds));
                    Data = await GetConfigurationFromRedisAsync();
                    OnReload();
                }
                catch { }
            }
        }

        public override void Load()
        {
            try
            {
                LoadAsync().GetAwaiter();
            }
            catch (JsonException e)
            {
                throw new FormatException("Could not parse the JSON file.", e);
            }
        }

        public async Task LoadAsync()
        {
            Data = await GetConfigurationFromRedisAsync();

            if (_handleRedisConfigurationTask.Status == TaskStatus.Created)
                _handleRedisConfigurationTask.Start();
        }

        private async Task<IDictionary<string, string?>> GetConfigurationFromRedisAsync()
        {
            IDictionary<string, string?> dict = new Dictionary<string, string?>();

            if (string.IsNullOrWhiteSpace(_options.ConnectionString))
            {
                return dict;
            }

            if (string.IsNullOrWhiteSpace(_options.ConfigKey))
            {
                return dict;
            }

            using RedisClient? redisClient = new(_options.ConnectionString);
            var value = redisClient.Get(_options.ConfigKey);

            if (string.IsNullOrWhiteSpace(value))
            {
                return dict;
            }

            await Task.Delay(1);
            return RedisConfigurationStringParser.Parse(value);
        }
    }
}
