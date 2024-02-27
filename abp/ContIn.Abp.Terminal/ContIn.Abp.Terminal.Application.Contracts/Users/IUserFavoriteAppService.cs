using Volo.Abp.Application.Services;

namespace ContIn.Abp.Terminal.Application.Contracts.Users
{
    /// <summary>
    /// 收藏
    /// </summary>
    public interface IUserFavoriteAppService : IApplicationService
    {
        /// <summary>
        /// 是否收藏
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="entityId">实体编号</param>
        /// <returns></returns>
        Task<bool> GetFavoritedAsync(string entityType, long entityId);
        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="entityId">实体编号</param>
        /// <returns></returns>
        Task DeletedAsync(string entityType, long entityId);
        /// <summary>
        /// 新增收藏
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="entityId">实体编号</param>
        /// <returns></returns>
        Task<UserFavoriteDto> InsertAsync(long userId, string entityType, long entityId);
    }
}
