using Volo.Abp.Domain.Repositories;

namespace ContIn.Abp.Terminal.Domain.Users
{
    /// <summary>
    /// 关注粉丝
    /// </summary>
    public interface IUserFollowRepository : IBasicRepository<UserFollow, long>
    {
        /// <summary>
        /// 是否关注
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="otherId"></param>
        /// <returns></returns>
        Task<bool> IsFollowedAsync(long userId, long otherId);
        /// <summary>
        /// 获取关注数据行
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="otherId"></param>
        /// <returns></returns>
        Task<UserFollow?> GetUserFollowAsync(long userId, long otherId);
        /// <summary>
        /// 获取关注列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cursor"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        Task<List<UserFollow>> GetFollowsAsync(long userId, long cursor, int limit = 10, bool includeDetails = false);
        /// <summary>
        /// 获取粉丝列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cursor"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        Task<List<UserFollow>> GetFansAsync(long userId, long cursor, int limit = 10, bool includeDetails = false);
    }
}
