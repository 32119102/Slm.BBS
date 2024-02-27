using ContIn.Abp.Terminal.Domain.Shared.Enum;
using Volo.Abp.Application.Services;

namespace ContIn.Abp.Terminal.Application.Contracts.Feed
{
    /// <summary>
    /// Feed
    /// </summary>
    public interface IUserFeedAppService : IApplicationService
    {
        /// <summary>
        /// 根据用户删除
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="authorId"></param>
        /// <returns></returns>
        Task DeleteByUserAsync(long userId, long authorId);
        /// <summary>
        /// 根据实体删除
        /// </summary>
        /// <param name="dataId"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        Task DeleteByDataIdAsync(long dataId, EntityTypeEnum dataType);
        /// <summary>
        /// 新增Feed流数据
        /// </summary>
        /// <param name="userId">实体作者ID</param>
        /// <param name="dataId">实体ID</param>
        /// <param name="dataType">实体类型</param>
        /// <returns></returns>
        Task CreateByUserAsync(long userId, long dataId, EntityTypeEnum dataType);
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cursor"></param>
        /// <param name="dataType"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<List<UserFeedDto>> GetUserFeedsAsync(long userId, long cursor, EntityTypeEnum dataType, int limit);
    }
}
