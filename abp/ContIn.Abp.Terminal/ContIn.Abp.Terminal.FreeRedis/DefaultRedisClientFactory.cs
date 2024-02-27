using FreeRedis;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace ContIn.Abp.Terminal.FreeRedis
{
    public class DefaultRedisClientFactory : IRedisClientFactory, ISingletonDependency
    {
        private readonly object _sync = new object();
        // redis客户端
        private readonly ConcurrentDictionary<string, RedisClient> _clientDict;
        protected ILogger Logger { get; }
        // redis配置提供程序
        protected IRedisConfigurationProvider ConfigurationSelector { get; }
        // redis实例化程序
        protected IRedisClientBuilder ClientBuilder { get; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configurationSelector"></param>
        /// <param name="clientBuilder"></param>
        public DefaultRedisClientFactory(ILogger<DefaultRedisClientFactory> logger, IRedisConfigurationProvider configurationSelector, IRedisClientBuilder clientBuilder)
        {
            Logger = logger;
            ConfigurationSelector = configurationSelector;
            ClientBuilder = clientBuilder;

            _clientDict = new ConcurrentDictionary<string, RedisClient>();
        }

        /// <summary>
        /// Get redis client by name
        /// </summary>
        /// <param name="name">name</param>
        /// <returns></returns>
        public virtual RedisClient Get(string name = DefaultClient.DefaultRedisClientName)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            if (!_clientDict.TryGetValue(name, out RedisClient client))
            {
                var configuration = ConfigurationSelector.Get(name);
                if (configuration == null)
                {
                    throw new AbpException($"Could not find configuration by name '{name}'");
                }

                lock (_sync)
                {
                    //Still can't find client
                    if (!_clientDict.TryGetValue(name, out client))
                    {
                        client = ClientBuilder.CreateClient(configuration);
                        if (_clientDict.TryAdd(name, client))
                        {
                            Logger.LogInformation("Create and add redis client '{0}',ConnectionString:'{1}',Mode:'{2}'.", name, configuration.ConnectionString, configuration.Mode);
                        }
                        else
                        {
                            Logger.LogWarning("Add client to dict fail! client name has exists! client name:{0}", name);
                        }
                    }
                }
            }

            return client;
        }

        /// <summary>
        /// Get all csredis client
        /// </summary>
        /// <returns></returns>
        public List<RedisClient> GetAll()
        {
            return _clientDict.Values.ToList();
        }
    }
}
