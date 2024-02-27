using FreeRedis;

namespace ContIn.Abp.Terminal.FreeRedis
{
    public static class RedisClientFactoryExtensions
    {
        public static RedisClient GetClient<TClient>(this IRedisClientFactory clientFactory)
        {
            return clientFactory.GetClient(RedisClientNameAttribute.GetClientName<TClient>());
        }

        public static RedisClient GetClient(this IRedisClientFactory clientFactory, string name)
        {
            return clientFactory.Get(name);
        }
    }
}
