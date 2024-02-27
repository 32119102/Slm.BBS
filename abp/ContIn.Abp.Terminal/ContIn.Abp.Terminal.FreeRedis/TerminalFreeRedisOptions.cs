using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace ContIn.Abp.Terminal.FreeRedis
{
    public class TerminalFreeRedisOptions
    {
        public const string Position = "FreeRedis";
        public FreeRedisConfigurations Clients { get; set; }

        public TerminalFreeRedisOptions()
        {
            Clients = new FreeRedisConfigurations();
        }

        /// <summary>
        /// 注册配置
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public TerminalFreeRedisOptions Configure(IConfiguration configuration)
        {
            var csRedisConfigurations = configuration.Get<Dictionary<string, FreeRedisConfiguration>>();

            foreach (var kv in csRedisConfigurations)
            {
                Clients.Configure(kv.Key, c =>
                {
                    c.Mode = kv.Value.Mode;
                    c.ConnectionString = kv.Value.ConnectionString;
                    c.Sentinels = kv.Value.Sentinels;
                    c.ReadOnly = kv.Value.ReadOnly;
                });
            }
            return this;
        }
    }
}
