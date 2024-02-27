using ContIn.Abp.Terminal.Application.Contracts.Users;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ContIn.Abp.Terminal.Application.Contracts.Topics
{
    /// <summary>
    /// 话题
    /// </summary>
    public interface ITopicAppService : IApplicationService
    {
        /// <summary>
        /// 列表查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<TopicDto>> PostPagedAsync(SearchTopicPagedDto input);
        /// <summary>
        /// 设置推荐
        /// </summary>
        /// <param name="id"></param>
        /// <param name="recommend"></param>
        /// <returns></returns>
        Task SetRecommendAsync(long id, bool recommend);
        /// <summary>
        /// 设置状态，0-正常 1-删除
        /// 只有管理员能操作
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task SetDeleteAsync(long id, string status);
        /// <summary>
        /// 删除话题
        /// 只有作者和管理员能操作
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task PostDeleteAsync(long id);

        /// <summary>
        /// 获取所有节点信息
        /// </summary>
        /// <returns></returns>
        Task<List<TopicNodeSimpleDto>> GetNodesAsync();
        /// <summary>
        /// 获取节点信息
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        Task<TopicNodeSimpleDto> GetNodeAsync(long nodeId);
        /// <summary>
        /// 获取用户话题列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cursor"></param>
        /// <returns></returns>
        Task<TopicPagedResultDto> GetUserTopicsAsync(long userId, long cursor);
        /// <summary>
        /// 获取话题列表
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="recommend"></param>
        /// <param name="cursor"></param>
        /// <returns></returns>
        Task<TopicPagedResultDto> GetTopicsAsync(long nodeId = 0, bool recommend = false, long cursor = 0);

        /// <summary>
        /// 获取置顶话题
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        Task<List<TopicSimpleDto>> GetStickyTopicsAsync(long nodeId = 0);

        /// <summary>
        /// 设置话题置顶
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="sticky"></param>
        /// <returns></returns>
        Task PostStickyAsync(long topicId, bool sticky = false);

        /// <summary>
        /// 获取标签相关文章列表
        /// </summary>
        /// <param name="tagId">标签编号</param>
        /// <param name="cursor">偏移编号</param>
        /// <returns></returns>
        Task<TopicPagedResultDto> GetTagTopicsAsync(long tagId, long cursor = 0);
        /// <summary>
        /// 获取话题详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TopicDetailDto> GetAsync(long id);
        /// <summary>
        /// 获取话题信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TopicSimpleDto?> GetSimpleAsync(long id);
        /// <summary>
        /// 获取最近点赞信息
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        Task<List<UserSimpleDto>> GetRecentLikedAsync(long topicId);

        /// <summary>
        /// 收藏
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        Task PostFavoriteAsync(long topicId);
        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        Task PostLikeAsync(long topicId);

        /// <summary>
        /// 新增话题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<TopicDetailDto> PostCreateAsync(CreateUpdateTopicDto input);
        /// <summary>
        /// 修改话题
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<TopicDetailDto> PutUpdateAsync(long id, CreateUpdateTopicDto input);


        /// <summary>
        /// 获取关注人的话题列表
        /// </summary>
        /// <param name="cursor">上一条话题的创建时间</param>
        /// <returns></returns>
        Task<TopicPagedResultDto> GetFeedsAsync(long cursor = 0);

        /// <summary>
        /// 移除话题缓存
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task RemoveCacheAsync(long id);
    }
}
