using ContIn.Abp.Terminal.Domain.Shared;
using ContIn.Abp.Terminal.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ContIn.Abp.Terminal.EntityFrameworkCore.Users
{
    /// <summary>
    /// 收藏
    /// </summary>
    public class UserFavoriteRepository : EfCoreRepository<TerminalDbContext, UserFavorite, long>, IUserFavoriteRepository
    {
        public UserFavoriteRepository(IDbContextProvider<TerminalDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async override Task<UserFavorite> InsertAsync(UserFavorite entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            var fs = await dbSet.Where(x => x.UserId == entity.UserId)
                .Where(x => x.EntityType == entity.EntityType!.ToLower())
                .Where(x => x.EntityId == entity.EntityId)
                .ToListAsync();
            if (fs != null && fs.Count > 0)
            {
                return fs.First();
            }
            return await base.InsertAsync(entity, autoSave, cancellationToken);
        }

        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entityType"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public async Task DeletedAsync(long userId, string entityType, long entityId)
        {
            var dbSet = await GetDbSetAsync();
            var fs = await dbSet.Where(x => x.UserId == userId)
                .Where(x => x.EntityType == entityType.ToLower())
                .Where(x => x.EntityId == entityId)
                .ToListAsync();
            if (fs != null)
            {
                fs.ForEach(async f =>
                {
                    await DeleteAsync(f);
                });
            }
        }

        /// <summary>
        /// 是否收藏
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entityType"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public async Task<bool> ExistsUserFavoritedAsync(long userId, string entityType, long entityId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(x => x.UserId == userId)
                .Where(x => x.EntityType == entityType.ToLower())
                .Where(x => x.EntityId == entityId)
                .AnyAsync();
        }

        /// <summary>
        /// 获取用户收藏
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entityType"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public async Task<UserFavorite?> GetUserFavoriteAsync(long userId, string entityType, long entityId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(x => x.UserId == userId)
                .Where(x => x.EntityType == entityType.ToLower())
                .Where(x => x.EntityId == entityId)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 获取用户收藏列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cursor"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<List<UserFavorite>> GetUserFavoritesAsync(long userId, long cursor, int limit = 10)
        {
            var dbSet = await GetDbSetAsync();   
            return await dbSet.Where(x => x.UserId == userId)
                .WhereIf(cursor > 0, x => x.Id < cursor)
                .OrderByDescending(x => x.Id)
                .Take(limit)
                .ToListAsync();
        }
    }
}
