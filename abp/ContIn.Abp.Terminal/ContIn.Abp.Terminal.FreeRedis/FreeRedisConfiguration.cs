using System.Collections.Generic;

namespace ContIn.Abp.Terminal.FreeRedis
{
    /// <summary>
    /// redis配置
    /// </summary>
    public class FreeRedisConfiguration
    {
        /// <summary>
        /// redis模式
        /// </summary>
        public RedisModeEnum Mode { get; set; } = RedisModeEnum.Single;
        /// <summary>
        /// redis地址
        /// </summary>
        public string ConnectionString { get; set; } = "127.0.0.1";
        /// <summary>
        /// 哨兵
        /// </summary>
        public List<string> Sentinels { get; set; }
        /// <summary>
        /// 是否只读
        /// </summary>
        public bool ReadOnly { get; set; } = false;
        /// <summary>
        /// 构造函数
        /// </summary>
        public FreeRedisConfiguration()
        {
            Sentinels = new List<string>();
        }
    }
}
