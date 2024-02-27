using Volo.Abp.Application.Services;

namespace ContIn.Abp.Terminal.Application.Contracts.Users
{
    /// <summary>
    /// 点赞
    /// </summary>
    public interface IUserLikeAppService : IApplicationService
    {
        /// <summary>
        /// 是否点赞
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="entityId">实体编号</param>
        /// <returns></returns>
        Task<bool> GetLikedAsync(string entityType, long entityId);

        /// <summary>
        /// 获取最近点赞
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="entityId">实体编号</param>
        /// <param name="limit">获取数量</param>
        /// <returns></returns>
        Task<List<UserLikeDto>> GetRecentAsync(string entityType, long entityId, int limit = 5);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<UserLikeDto> InsertAsync(CreateUserLikeDto input);
    }
}
