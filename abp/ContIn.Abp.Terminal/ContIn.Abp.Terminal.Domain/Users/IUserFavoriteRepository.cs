using Volo.Abp.Domain.Repositories;

namespace ContIn.Abp.Terminal.Domain.Users
{
    /// <summary>
    /// 收藏
    /// </summary>
    public interface IUserFavoriteRepository : IBasicRepository<UserFavorite, long>
    {
        /// <summary>
        /// 是否收藏
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="entityId">实体编号</param>
        /// <returns></returns>
        Task<bool> ExistsUserFavoritedAsync(long userId, string entityType, long entityId);
        /// <summary>
        /// 获取用户收藏
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="entityId">实体编号</param>
        /// <returns></returns>
        Task<UserFavorite?> GetUserFavoriteAsync(long userId, string entityType, long entityId);
        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="entityId">实体编号</param>
        /// <returns></returns>
        Task DeletedAsync(long userId, string entityType, long entityId);
        /// <summary>
        /// 获取用户收藏列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cursor"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<List<UserFavorite>> GetUserFavoritesAsync(long userId, long cursor, int limit = 10);
    }
}
