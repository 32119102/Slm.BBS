using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ContIn.Abp.Terminal.Application.Contracts.Articles
{
    /// <summary>
    /// 文章
    /// </summary>
    public interface IArticleAppService : IApplicationService
    {
        /// <summary>
        /// 列表查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ArticleDto>> PostPagedAsync(SearchArticlePagedDto input);
        /// <summary>
        /// 查询文章标签信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<ArticleTagDto>> GetTagsAsync(long id);
        /// <summary>
        /// 修改文章标签
        /// </summary>
        /// <param name="id">文章编号</param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<ArticleTagDto>> UpdateTagsAsync(long id, UpdateArticleTagDto input);
        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="id">文章编号</param>
        /// <returns></returns>
        Task DeleteAsync(long id);
        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="id">文章编号</param>
        /// <returns></returns>
        Task PendingAsync(long id);
        /// <summary>
        /// 获取用户文章列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cursor"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<SitePagedResultDto<ArticleSimpleDto>> GetUserArticlesAsync(long userId, long cursor = 0, int limit = 10);
        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="cursor">偏移量</param>
        /// <param name="limit">显示数</param>
        /// <returns></returns>
        Task<SitePagedResultDto<ArticleSimpleDto>> GetArticlesAsync(long cursor, int limit = 10);
        /// <summary>
        /// 获取文章详情
        /// </summary>
        /// <param name="id">文章编号</param>
        /// <returns></returns>
        Task<ArticleDetailDto> GetAsync(long id);
        /// <summary>
        /// 获取文章信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ArticleSimpleDto?> GetSimpleAsync(long id);
        /// <summary>
        /// 获取近期文章
        /// </summary>
        /// <param name="articleId">文章编号</param>
        /// <returns></returns>
        Task<List<ArticleSimpleDto>> GetNearlyAsync(long articleId);
        /// <summary>
        /// 获取相关文章
        /// </summary>
        /// <param name="articleId">文章编号</param>
        /// <returns></returns>
        Task<List<ArticleSimpleDto>> GetRelatedAsync(long articleId);
        /// <summary>
        /// 收藏
        /// </summary>
        /// <param name="articleId">文章编号</param>
        /// <returns></returns>
        Task PostFavoriteAsync(long articleId);

        /// <summary>
        /// 新增文章
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ArticleSimpleDto> PostCreateAsync(CreateUpdateArticleDto input);
        /// <summary>
        /// 修改文章
        /// </summary>
        /// <param name="id">文章编号</param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ArticleSimpleDto> UpdateAsync(long id, CreateUpdateArticleDto input);
        /// <summary>
        /// 移除文章缓存
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task RemoveCacheAsync(long id);
    }
}
