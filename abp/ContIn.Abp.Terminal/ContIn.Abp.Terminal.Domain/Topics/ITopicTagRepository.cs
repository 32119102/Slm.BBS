using Volo.Abp.Domain.Repositories;

namespace ContIn.Abp.Terminal.Domain.Topics
{
    /// <summary>
    /// 话题标签
    /// </summary>
    public interface ITopicTagRepository : IBasicRepository<TopicTag, long>
    {
        /// <summary>
        /// 保存话题标签关系
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="tagIds"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        Task AddTopicTags(long topicId, long[] tagIds, long time);
        /// <summary>
        /// 获取标签相关文章
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="cursor"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<List<TopicTag>> GetTagTopicsAsync(long tagId, long cursor, int limit = 10);
        /// <summary>
        /// 设置删除状态
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        Task SetDeleteAsync(long topicId);
    }
}
