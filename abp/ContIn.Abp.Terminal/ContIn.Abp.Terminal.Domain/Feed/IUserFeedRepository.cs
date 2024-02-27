using ContIn.Abp.Terminal.Domain.Shared.Enum;
using Volo.Abp.Domain.Repositories;

namespace ContIn.Abp.Terminal.Domain.Feed
{
    /// <summary>
    /// Feed
    /// </summary>
    public interface IUserFeedRepository : IBasicRepository<UserFeed, long>
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
        /// 获取数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cursor"></param>
        /// <param name="dataType"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<List<UserFeed>?> GetUserFeedsAsync(long userId, long cursor, EntityTypeEnum dataType, int limit = 10);
    }
}
