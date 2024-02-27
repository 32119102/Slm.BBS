using ContIn.Abp.Terminal.Domain.Topics;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ContIn.Abp.Terminal.EntityFrameworkCore.Topics
{
    /// <summary>
    /// 节点仓储接口实现
    /// </summary>
    public class TopicNodeRepository : EfCoreRepository<TerminalDbContext, TopicNode, long>, ITopicNodeRepository
    {
        public TopicNodeRepository(IDbContextProvider<TerminalDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        /// <summary>
        /// 根据名称获取信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<TopicNode?> FindByNameAsync(string name)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(x => x.Name == name);
        }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<int> GetCountAsync(string? name)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .WhereIf(
                    !name.IsNullOrWhiteSpace(),
                    x => x.Name!.Contains(name!)
                )
                .CountAsync();
        }
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<List<TopicNode>> GetListAsync(int skipCount, int maxResultCount, string? name)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .WhereIf(
                    !name.IsNullOrWhiteSpace(),
                    x => x.Name!.Contains(name!)
                )
                .OrderBy(x => x.SortNo)
                // .ThenByDescending(x => x.Id)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }
    }
}
