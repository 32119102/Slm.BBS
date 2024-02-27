using ContIn.Abp.Terminal.Domain.Links;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ContIn.Abp.Terminal.EntityFrameworkCore.Links
{
    /// <summary>
    /// 友情链接
    /// </summary>
    public class FriendlyLinkRepository : EfCoreRepository<TerminalDbContext, FriendlyLink, long>, IFriendlyLinkRepository
    {
        public FriendlyLinkRepository(IDbContextProvider<TerminalDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <param name="url"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public async Task<int> GetCountAsync(string? url, string? title)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .WhereIf(!url.IsNullOrWhiteSpace(), link => link.Url!.Contains(url!))
                .WhereIf(!title.IsNullOrWhiteSpace(), link => link.Title!.Contains(title!))
                .CountAsync();
        }
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="url"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public async Task<List<FriendlyLink>> GetListAsync(int skipCount, int maxResultCount, string? url, string? title)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .WhereIf(!url.IsNullOrWhiteSpace(), link => link.Url!.Contains(url!))
                .WhereIf(!title.IsNullOrWhiteSpace(), link => link.Title!.Contains(title!))
                .OrderByDescending(x => x.Id)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<(int, List<FriendlyLink>)> GetPagingAsync(int page = 1, int limit = 20)
        {
            var dbSet = await GetDbSetAsync();
            var count = await dbSet.Where(x => x.Status == Domain.Shared.Enum.StatusEnum.Normal).CountAsync();
            var list = await dbSet.Where(x => x.Status == Domain.Shared.Enum.StatusEnum.Normal)
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
            return (count, list);
        }

        /// <summary>
        /// 获取前10个链接
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<List<FriendlyLink>> GetTopLinksAsync(int limit = 10)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .OrderBy(x => x.Id)
                .Take(limit)
                .ToListAsync();
        }
    }
}
