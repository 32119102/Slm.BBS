using FreeRedis;
using System.Collections.Generic;

namespace ContIn.Abp.Terminal.FreeRedis
{
    public interface IRedisClientFactory
    {
        RedisClient Get(string name = DefaultClient.DefaultRedisClientName);

        List<RedisClient> GetAll();
    }
}
