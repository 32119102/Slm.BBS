using ContIn.Abp.Terminal.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ContIn.Abp.Terminal.EntityFrameworkCore.Users
{
    /// <summary>
    /// 点赞
    /// </summary>
    public class UserLikeRepository : EfCoreRepository<TerminalDbContext, UserLike, long>, IUserLikeRepository
    {
        public UserLikeRepository(IDbContextProvider<TerminalDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        /// <summary>
        /// 是否点赞
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entityType"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public async Task<bool> ExistsUserLikeAsync(long userId, string entityType, long entityId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(x => x.UserId == userId)
                .Where(x => x.EntityType == entityType.ToLower())
                .Where(x => x.EntityId == entityId)
                .AnyAsync();
        }

        /// <summary>
        /// 获取用户点赞
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entityType"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public async Task<UserLike?> GetLikedAsync(long userId, string entityType, long entityId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(x => x.UserId == userId)
                .Where(x => x.EntityType == entityType.ToLower())
                .Where(x => x.EntityId == entityId)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 获取最近点赞信息
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="entityId"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        public async Task<List<UserLike>> GetRecentAsync(string entityType, long entityId, int limit = 5, bool includeDetails = false)
        {
            var dbSet = await GetDbSetAsync();
            var likes = await dbSet
                .Where(x => x.EntityType == entityType.ToLower())
                .Where(x => x.EntityId == entityId)
                .OrderByDescending(x => x.Id)
                .Take(limit)
                .ToListAsync();
            if (includeDetails)
            {
                var dbContext = await GetDbContextAsync();
                likes.ForEach(like =>
                {
                    dbContext.Entry(like).Reference(x => x.User).Load();
                });
            }
            return likes;
        }
    }
}
