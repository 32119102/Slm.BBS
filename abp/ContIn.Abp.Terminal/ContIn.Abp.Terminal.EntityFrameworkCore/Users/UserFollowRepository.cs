using ContIn.Abp.Terminal.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ContIn.Abp.Terminal.EntityFrameworkCore.Users
{
    /// <summary>
    /// 关注粉丝
    /// </summary>
    public class UserFollowRepository : EfCoreRepository<TerminalDbContext, UserFollow, long>, IUserFollowRepository
    {
        public UserFollowRepository(IDbContextProvider<TerminalDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        /// <summary>
        /// 获取粉丝列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cursor"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        public async Task<List<UserFollow>> GetFansAsync(long userId, long cursor, int limit = 10, bool includeDetails = false)
        {
            var dbSet = await GetDbSetAsync();
            var fans = await dbSet.Where(x => x.OtherId == userId)
                .WhereIf(cursor > 0, x => x.Id < cursor)
                .OrderByDescending(x => x.Id)
                .Take(limit)
                .ToListAsync();
            if (includeDetails)
            {
                var dbContext = await GetDbContextAsync();
                fans.ForEach(x =>
                {
                    dbContext.Entry(x).Reference(m => m.User).Load();
                });
            }
            return fans;
        }

        /// <summary>
        /// 获取关注列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cursor"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        public async Task<List<UserFollow>> GetFollowsAsync(long userId, long cursor, int limit = 10, bool includeDetails = false)
        {
            var dbSet = await GetDbSetAsync();
            var follows = await dbSet.Where(x => x.UserId == userId)
                .WhereIf(cursor > 0, x => x.Id < cursor)
                .OrderByDescending(x => x.Id)
                .Take(limit)
                .ToListAsync();
            if (includeDetails)
            {
                var dbContext = await GetDbContextAsync();
                follows.ForEach(x =>
                {
                    dbContext.Entry(x).Reference(m => m.Other).Load();
                });
            }
            return follows;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="otherId"></param>
        /// <returns></returns>
        public async Task<UserFollow?> GetUserFollowAsync(long userId, long otherId)
        {
            if (userId == otherId) return default;
            var dbSet = await GetDbSetAsync();
            return await dbSet.Where(x => x.UserId == userId)
                .Where(x => x.OtherId == otherId)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 是否关注
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="otherId"></param>
        /// <returns></returns>
        public async Task<bool> IsFollowedAsync(long userId, long otherId)
        {
            if (userId == otherId)
            {
                return false;
            }
            var dbSet = await GetDbSetAsync();
            return await dbSet.Where(x => x.UserId == userId)
                .Where(x => x.OtherId == otherId)
                .AnyAsync();
        }
    }
}
