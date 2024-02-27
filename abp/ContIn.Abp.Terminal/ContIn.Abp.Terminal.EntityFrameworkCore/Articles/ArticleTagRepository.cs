using ContIn.Abp.Terminal.Domain.Articles;
using ContIn.Abp.Terminal.Domain.Shared.Enum;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ContIn.Abp.Terminal.EntityFrameworkCore.Articles
{
    /// <summary>
    /// 文章标签
    /// </summary>
    public class ArticleTagRepository : EfCoreRepository<TerminalDbContext, ArticleTag, long>, IArticleTagRepository
    {
        public ArticleTagRepository(IDbContextProvider<TerminalDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        /// <summary>
        /// 新增文章标签关系
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="tagIds"></param>
        /// <param name="currentTime"></param>
        /// <returns></returns>
        public async Task AddArticleTagsAsync(long articleId, long[] tagIds, long currentTime)
        {
            foreach (var tag in tagIds)
            {
                await InsertAsync(new ArticleTag()
                {
                    ArticleId = articleId,
                    TagId = tag,
                    Status = StatusEnum.Normal,
                    CreateTime = currentTime
                });
            }
        }

        /// <summary>
        /// 获取文章标签
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        public async Task<List<ArticleTag>?> GetArticleTagsByArticleIdAsync(long articleId, bool includeDetails = false)
        {
            var dbSet = await GetDbSetAsync();
            var tags = await dbSet.Where(x => x.ArticleId == articleId).ToListAsync();

            if (includeDetails)
            {
                var context = await GetDbContextAsync();
                tags.ForEach(t =>
                {
                    context.Entry(t).Reference(x => x.Tag).Load();
                });
            }
            return tags;
        }

        /// <summary>
        /// 获取文章标签信息
        /// </summary>
        /// <param name="tagIds"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        public async Task<List<ArticleTag>> GetArticleTagsByTagIdsAsync(long[] tagIds, int limit = 30, bool includeDetails = false)
        {
            if (tagIds.Length == 0)
                return new List<ArticleTag>();

            var dbSet = await GetDbSetAsync();
            var tags = await dbSet.Where(x => tagIds.Contains(x.TagId))
                .Take(limit)
                .ToListAsync();
            if (includeDetails)
            {
                var context = await GetDbContextAsync();
                tags.ForEach(t =>
                {
                    context.Entry(t).Reference(x => x.Tag).Load();
                });
            }
            return tags;
        }

        /// <summary>
        /// 修改文章标签
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="tagIds"></param>
        /// <param name="currentTime"></param>
        /// <returns></returns>
        public async Task UpdateArticleTagAsync(long articleId, long[] tagIds, long currentTime)
        {
            var dbSet = await GetDbSetAsync();
            var existing = await dbSet.Where(x => x.ArticleId == articleId).ToListAsync();
            existing?.ForEach(async x =>
            {
                await DeleteAsync(x);
            });
            List<ArticleTag> tags = new List<ArticleTag>();
            tagIds.ToList().ForEach(tagId =>
            {
                tags.Add(new ArticleTag()
                {
                    ArticleId = articleId,
                    TagId = tagId,
                    Status = StatusEnum.Normal,
                    CreateTime = currentTime
                });
            });
            if (tags.Count > 0)
            {
                await InsertManyAsync(tags);
            }
            await SaveChangesAsync(CancellationToken.None);
        }
    }
}
