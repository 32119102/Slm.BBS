using System;
using System.Collections.Generic;
using Volo.Abp;

namespace ContIn.Abp.Terminal.FreeRedis
{
    public class FreeRedisConfigurations
    {
        // default
        private FreeRedisConfiguration Default => GetConfiguration<DefaultClient>();
        // 配置集合
        private readonly Dictionary<string, FreeRedisConfiguration> _configurations;
        /// <summary>
        /// 构造函数
        /// 默认添加 default 配置
        /// </summary>
        public FreeRedisConfigurations()
        {
            _configurations = new Dictionary<string, FreeRedisConfiguration>()
            {
                [RedisClientNameAttribute.GetClientName<DefaultClient>()] = new FreeRedisConfiguration()
            };
        }

        public FreeRedisConfigurations Configure<TClient>(Action<FreeRedisConfiguration> configureAction)
        {
            return Configure(RedisClientNameAttribute.GetClientName<TClient>(), configureAction);
        }

        public FreeRedisConfigurations Configure(string name, Action<FreeRedisConfiguration> configureAction)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNull(configureAction, nameof(configureAction));

            configureAction(
                _configurations.GetOrAdd(
                    name,
                    () => new FreeRedisConfiguration()
                    {
                        ConnectionString = Default.ConnectionString,
                        Mode = Default.Mode,
                        Sentinels = Default.Sentinels,
                        ReadOnly = Default.ReadOnly
                    }
                )
            );

            return this;
        }

        public FreeRedisConfigurations ConfigureDefault(Action<FreeRedisConfiguration> configureAction)
        {
            configureAction(Default);
            return this;
        }

        public FreeRedisConfigurations ConfigureAll(Action<string, FreeRedisConfiguration> configureAction)
        {
            foreach (var configuration in _configurations)
            {
                configureAction(configuration.Key, configuration.Value);
            }
            return this;
        }

        /// <summary>
        /// get configuration
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <returns></returns>
        public FreeRedisConfiguration GetConfiguration<TClient>()
        {
            return GetConfiguration(RedisClientNameAttribute.GetClientName<TClient>());
        }
        /// <summary>
        /// get configuration
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FreeRedisConfiguration GetConfiguration(string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            return _configurations.GetOrDefault(name) ?? Default;
        }
    }
}
