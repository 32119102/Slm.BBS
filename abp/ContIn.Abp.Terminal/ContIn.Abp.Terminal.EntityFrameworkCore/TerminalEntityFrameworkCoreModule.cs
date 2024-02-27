using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace ContIn.Abp.Terminal.EntityFrameworkCore
{
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    public class TerminalEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<TerminalDbContext>(options =>
            {
                // 为所有聚合根创建默认仓储
                // 推荐为每个聚合根定义仓储接口并创建对应的实现，而不是使用泛型仓储
                // 推荐 仓储接口 继承自 IBasicRepository<TEntity, TKey>
                // options.AddDefaultRepositories(includeAllEntities: false);
            });

            context.Services.Configure<AbpDbContextOptions>(options => options.UseMySQL());
        }
    }
}
