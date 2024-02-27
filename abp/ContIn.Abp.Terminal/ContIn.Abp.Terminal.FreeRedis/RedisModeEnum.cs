namespace ContIn.Abp.Terminal.FreeRedis
{
    /// <summary>
    /// redis模式
    /// </summary>
    public enum RedisModeEnum
    {
        Single = 1,

        Sentinel = 2,

        Cluster = 4
    }
}
