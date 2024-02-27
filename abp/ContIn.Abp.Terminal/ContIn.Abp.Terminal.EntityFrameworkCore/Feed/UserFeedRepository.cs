using ContIn.Abp.Terminal.Domain.Feed;
using ContIn.Abp.Terminal.Domain.Shared.Enum;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ContIn.Abp.Terminal.EntityFrameworkCore.Feed
{
    /// <summary>
    /// Feed
    /// </summary>
    public class UserFeedRepository : EfCoreRepository<TerminalDbContext, UserFeed, long>, IUserFeedRepository
    {
        public UserFeedRepository(IDbContextProvider<TerminalDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        /// <summary>
        /// 根据实体删除
        /// </summary>
        /// <param name="dataId"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public async Task DeleteByDataIdAsync(long dataId, EntityTypeEnum dataType)
        {
            var dbSet = await GetDbSetAsync();
            var feeds = await dbSet.Where(x => x.DataId == dataId)
                .Where(x => x.DataType == dataType.ToString().ToLower())
                .ToListAsync();
            if (feeds != null)
            {
                foreach (var f in feeds)
                {
                    await DeleteAsync(f.Id);
                }
            }
        }
        /// <summary>
        /// 根据用户删除
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="authorId"></param>
        /// <returns></returns>
        public async Task DeleteByUserAsync(long userId, long authorId)
        {
            var dbSet = await GetDbSetAsync();
            var feeds = await dbSet.Where(x => x.UserId == userId)
                .Where(x => x.AuthorId == authorId)
                .ToListAsync();
            if (feeds != null)
            {
                foreach (var f in feeds)
                {
                    await DeleteAsync(f.Id);
                }
            }
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cursor"></param>
        /// <param name="dataType"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<List<UserFeed>?> GetUserFeedsAsync(long userId, long cursor, EntityTypeEnum dataType, int limit = 10)
        {
            var dbSet = await GetDbSetAsync();
            var feeds = await dbSet.Where(x => x.UserId == userId)
                .Where(x => x.DataType == dataType.ToString().ToLower())
                .WhereIf(cursor > 0, f => f.CreateTime < cursor)
                .OrderByDescending(x => x.CreateTime)
                .Take(limit)
                .ToListAsync();
            return feeds;
        }
    }
}
