using ContIn.Abp.Terminal.Domain.Shared.Enum;
using Volo.Abp.Domain.Repositories;

namespace ContIn.Abp.Terminal.Domain.Articles
{
    /// <summary>
    /// 文章
    /// </summary>
    public interface IArticleRepository : IBasicRepository<Article, long>
    {
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="userIds"></param>
        /// <param name="title"></param>
        /// <param name="status"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        Task<List<Article>> GetListAsync(int skipCount, int maxResultCount, long[]? userIds, string? title, StatusEnum? status, bool includeDetails = false);
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="userIds">用户编号</param>
        /// <param name="title">标题</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        Task<int> GetCountAsync(long[]? userIds, string? title, StatusEnum? status);
        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task PendingToNormalAsync(long id);
        /// <summary>
        /// 获取用户文章列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cursor"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        Task<List<Article>> GetUserArticlesAsync(long userId, long cursor, int limit = 20, bool includeDetails = false);
        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="cursor"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        Task<List<Article>> GetArticlesAsync(long cursor, int limit = 20, bool includeDetails = false);
        /// <summary>
        /// 增加浏览量
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task IncrViewCount(long id);
        /// <summary>
        /// 获取近期文章
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        Task<List<Article>> GetNearlyArticlesAsync(long articleId, int limit = 10, bool includeDetails = false);
    }
}
