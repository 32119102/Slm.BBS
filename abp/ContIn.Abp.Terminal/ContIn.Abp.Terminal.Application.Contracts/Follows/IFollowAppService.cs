using Volo.Abp.Application.Services;

namespace ContIn.Abp.Terminal.Application.Contracts.Follows
{
    /// <summary>
    /// 粉丝关注
    /// </summary>
    public interface IFollowAppService : IApplicationService
    {
        /// <summary>
        /// 是否关注
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> IsFollowedAsync(long userId);
        /// <summary>
        /// 是否关注，有缓存
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="otherUserId"></param>
        /// <returns></returns>
        Task<UserFollowDto> GetUserFollowedAsync(long userId, long otherUserId);
        /// <summary>
        /// 获取用户关注列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cursor"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<UserFollowFansDto> GetFollowsAsync(long userId, long cursor = 0, int limit = 5);
        /// <summary>
        /// 获取用户粉丝列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cursor"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<UserFollowFansDto> GetFansAsync(long userId, long cursor = 0, int limit = 5);
        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task PostFollowAsync(UserFollowUpdateDto input);
        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task PostUnFollowAsync(UserFollowUpdateDto input);
    }
}
