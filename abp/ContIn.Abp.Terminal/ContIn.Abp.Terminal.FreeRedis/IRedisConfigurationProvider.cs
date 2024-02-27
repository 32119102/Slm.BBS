namespace ContIn.Abp.Terminal.FreeRedis
{
    public interface IRedisConfigurationProvider
    {
        FreeRedisConfiguration Get(string name);
    }
}
