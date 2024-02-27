using Volo.Abp.Domain.Repositories;

namespace ContIn.Abp.Terminal.Domain.Users
{
    /// <summary>
    /// 点赞
    /// </summary>
    public interface IUserLikeRepository : IBasicRepository<UserLike, long>
    {
        /// <summary>
        /// 是否点赞
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entityType"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        Task<bool> ExistsUserLikeAsync(long userId, string entityType, long entityId);
        /// <summary>
        /// 获取用户点赞
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entityType"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        Task<UserLike?> GetLikedAsync(long userId, string entityType, long entityId);
        /// <summary>
        /// 获取最近点赞信息
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="entityId"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        Task<List<UserLike>> GetRecentAsync(string entityType, long entityId, int limit = 5, bool includeDetails = false);
    }
}
