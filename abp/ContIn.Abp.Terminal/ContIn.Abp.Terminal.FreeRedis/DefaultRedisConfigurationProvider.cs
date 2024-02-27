using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace ContIn.Abp.Terminal.FreeRedis
{
    public class DefaultRedisConfigurationProvider : IRedisConfigurationProvider, ITransientDependency
    {
        protected TerminalFreeRedisOptions Options { get; }

        public DefaultRedisConfigurationProvider(IOptions<TerminalFreeRedisOptions> options)
        {
            Options = options.Value;
        }

        public virtual FreeRedisConfiguration Get(string name)
        {
            return Options.Clients.GetConfiguration(name);
        }
    }
}
