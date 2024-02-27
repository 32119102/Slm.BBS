using ContIn.Abp.Terminal.Domain.Shared.Enum;
using Volo.Abp.Domain.Repositories;

namespace ContIn.Abp.Terminal.Domain.Topics
{
    /// <summary>
    /// 话题
    /// </summary>
    public interface ITopicRepository : IBasicRepository<Topic, long>
    {
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="userIds"></param>
        /// <param name="title"></param>
        /// <param name="recommend">是否推荐</param>
        /// <param name="sticky"></param>
        /// <param name="status"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        Task<List<Topic>> GetListAsync(int skipCount, int maxResultCount, long[]? userIds, string? title, bool? recommend, bool? sticky, StatusEnum? status, bool includeDetails = false);
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="userIds">用户编号</param>
        /// <param name="title">标题</param>
        /// <param name="recommend">是否推荐</param>
        /// <param name="sticky"></param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        Task<int> GetCountAsync(long[]? userIds, string? title, bool? recommend, bool? sticky, StatusEnum? status);
        /// <summary>
        /// 获取用户话题
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cursor"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        Task<List<Topic>> GetUserTopicsAsync(long userId, long cursor, int limit = 20, bool includeDetails = false);
        /// <summary>
        /// 获取话题列表
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="recommend"></param>
        /// <param name="cursor"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        Task<List<Topic>> GetPagedTopicsAsync(long nodeId, bool recommend, long cursor, int limit = 20, bool includeDetails = false);

        /// <summary>
        /// 获取置顶话题
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        Task<List<Topic>> GetStickyTopicsAsync(long nodeId, int limit = 3, bool includeDetails = false);

        /// <summary>
        /// 设置话题置顶
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="sticky"></param>
        /// <param name="stickyTime"></param>
        /// <returns></returns>
        Task SetStickyAsync(long topicId, bool sticky, long stickyTime);

        /// <summary>
        /// 增加浏览量
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task IncrViewCount(long id);
        /// <summary>
        /// 增加点赞数量
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task IncrLikeCountAsync(long id);

        /// <summary>
        /// 当帖子被评论的时候，更新最后回复时间、回复数量+1
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="userId"></param>
        /// <param name="commentCreateTime"></param>
        /// <returns></returns>
        Task OnCommentAsync(long topicId, long userId, long commentCreateTime);

        /// <summary>
        /// 话题搜索
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="nodeId"></param>
        /// <param name="recommend"></param>
        /// <param name="minCreateTime"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        Task<(int, List<Topic>)> SearchTopicsAsync(string keyword, long nodeId, bool recommend, long minCreateTime, int page, int limit, bool includeDetails = false);
    }
}
