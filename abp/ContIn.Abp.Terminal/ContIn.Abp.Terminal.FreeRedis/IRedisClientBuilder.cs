using FreeRedis;

namespace ContIn.Abp.Terminal.FreeRedis
{
    public interface IRedisClientBuilder
    {
        RedisClient CreateClient(FreeRedisConfiguration configuration);
    }
}
