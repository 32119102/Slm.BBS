using ContIn.Abp.Terminal.Domain.Articles;
using ContIn.Abp.Terminal.Domain.Shared.Enum;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ContIn.Abp.Terminal.EntityFrameworkCore.Articles
{
    /// <summary>
    /// 文章
    /// </summary>
    public class ArticleRepository : EfCoreRepository<TerminalDbContext, Article, long>, IArticleRepository
    {
        public ArticleRepository(IDbContextProvider<TerminalDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        /// <summary>
        /// 获取文章详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async override Task<Article> GetAsync(long id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            var article = await base.GetAsync(id, includeDetails, cancellationToken);
            if (includeDetails)
            {
                var context = await GetDbContextAsync();
                context.Entry(article).Reference(x => x.User).Load();
                context.Entry(article).Collection(x => x.ArticleTags!).Load();

                article.ArticleTags?.ToList().ForEach(m =>
                {
                    context.Entry(m).Reference(x => x.Tag).Load();
                });
            }
            return article;
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="title"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<int> GetCountAsync(long[]? userIds, string? title, StatusEnum? status)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .WhereIf(userIds != null && userIds.Length > 0, x => userIds!.Contains(x.UserId))
                .WhereIf(!title.IsNullOrWhiteSpace(), x => x.Title!.Contains(title!))
                .WhereIf(status != null, x => x.Status == status)
                .CountAsync();
        }
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
        public async Task<List<Article>> GetListAsync(int skipCount, int maxResultCount, long[]? userIds, string? title, StatusEnum? status, bool includeDetails = false)
        {
            var dbSet = await GetDbSetAsync();
            var articles = await dbSet
                .WhereIf(userIds != null && userIds.Length > 0, x => userIds!.Contains(x.UserId))
                .WhereIf(!title.IsNullOrWhiteSpace(), x => x.Title!.Contains(title!))
                .WhereIf(status != null, x => x.Status == status)
                .OrderByDescending(x => x.Id)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();

            if (includeDetails)
            {
                var context = await GetDbContextAsync();
                articles.ForEach(t =>
                {
                    context.Entry(t).Reference(x => x.User).Load();
                    context.Entry(t).Collection(x => x.ArticleTags!).Load();

                    t.ArticleTags?.ToList().ForEach(m =>
                    {
                        context.Entry(m).Reference(x => x.Tag).Load();
                    });
                });
            }
            return articles;
        }

        /// <summary>
        /// 重写删除逻辑
        /// </summary>
        /// <param name="id"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task DeleteAsync(long id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var t = await GetAsync(id, false, cancellationToken);

            t.Status = StatusEnum.Delete;

            await UpdateAsync(t, autoSave, cancellationToken);
        }

        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task PendingToNormalAsync(long id)
        {
            var t = await GetAsync(id, false);

            t.Status = StatusEnum.Normal;

            await UpdateAsync(t);
        }
        /// <summary>
        /// 获取用户文章列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cursor"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        public async Task<List<Article>> GetUserArticlesAsync(long userId, long cursor, int limit = 20, bool includeDetails = false)
        {
            var dbSet = await GetDbSetAsync();
            var articles = await dbSet
                .Where(x => x.Status == StatusEnum.Normal)
                .Where(x => x.UserId == userId)
                .WhereIf(cursor > 0, x => x.Id < cursor)
                .OrderByDescending(x => x.Id)
                .Take(limit)
                .ToListAsync();
            if (includeDetails)
            {
                var context = await GetDbContextAsync();
                articles.ForEach(t =>
                {
                    context.Entry(t).Reference(x => x.User).Load();
                    context.Entry(t).Collection(x => x.ArticleTags!).Load();

                    t.ArticleTags?.ToList().ForEach(m =>
                    {
                        context.Entry(m).Reference(x => x.Tag).Load();
                    });
                });
            }
            return articles;
        }
        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="cursor"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        public async Task<List<Article>> GetArticlesAsync(long cursor, int limit = 20, bool includeDetails = false)
        {
            var dbSet = await GetDbSetAsync();
            var articles = await dbSet
                .Where(x => x.Status == StatusEnum.Normal)
                .WhereIf(cursor > 0, x => x.Id < cursor)
                .OrderByDescending(x => x.Id)
                .Take(limit)
                .ToListAsync();
            if (includeDetails)
            {
                var context = await GetDbContextAsync();
                articles.ForEach(t =>
                {
                    context.Entry(t).Reference(x => x.User).Load();
                    context.Entry(t).Collection(x => x.ArticleTags!).Load();

                    t.ArticleTags?.ToList().ForEach(m =>
                    {
                        context.Entry(m).Reference(x => x.Tag).Load();
                    });
                });
            }
            return articles;
        }

        /// <summary>
        /// 增加浏览量
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task IncrViewCount(long id)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"update t_article set view_count = view_count + 1 where id = {id}");
        }

        /// <summary>
        /// 获取近期文章
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        public async Task<List<Article>> GetNearlyArticlesAsync(long articleId, int limit = 10, bool includeDetails = false)
        {
            var dbSet = await GetDbSetAsync();
            var articles = await dbSet.Where(x => x.Id < articleId)
                .Where(x => x.Status == StatusEnum.Normal)
                .OrderByDescending(x => x.Id)
                .Take(limit)
                .ToListAsync();
            if (includeDetails)
            {
                var context = await GetDbContextAsync();
                articles.ForEach(t =>
                {
                    context.Entry(t).Reference(x => x.User).Load();
                    context.Entry(t).Collection(x => x.ArticleTags!).Load();

                    t.ArticleTags?.ToList().ForEach(m =>
                    {
                        context.Entry(m).Reference(x => x.Tag).Load();
                    });
                });
            }
            return articles;
        }
    }
}
