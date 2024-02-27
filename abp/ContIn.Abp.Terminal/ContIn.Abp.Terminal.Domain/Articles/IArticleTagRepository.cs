using Volo.Abp.Domain.Repositories;

namespace ContIn.Abp.Terminal.Domain.Articles
{
    /// <summary>
    /// 文章标签
    /// </summary>
    public interface IArticleTagRepository : IBasicRepository<ArticleTag, long>
    {
        /// <summary>
        /// 获取文章标签信息
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        Task<List<ArticleTag>?> GetArticleTagsByArticleIdAsync(long articleId, bool includeDetails = false);
        /// <summary>
        /// 修改文章标签
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="tagIds"></param>
        /// <param name="currentTime"></param>
        /// <returns></returns>
        Task UpdateArticleTagAsync(long articleId, long[] tagIds, long currentTime);
        /// <summary>
        /// 获取文章标签信息
        /// </summary>
        /// <param name="tagIds"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        Task<List<ArticleTag>> GetArticleTagsByTagIdsAsync(long[] tagIds, int limit = 30, bool includeDetails = false);

        /// <summary>
        /// 添加文字标签关系
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="tagIds"></param>
        /// <param name="currentTime"></param>
        /// <returns></returns>
        Task AddArticleTagsAsync(long articleId, long[] tagIds, long currentTime);
    }
}
