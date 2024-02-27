using ContIn.Abp.Terminal.Domain.Comments;
using ContIn.Abp.Terminal.Domain.Shared.Enum;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ContIn.Abp.Terminal.EntityFrameworkCore.Comments
{
    /// <summary>
    /// 评论
    /// </summary>
    public class CommentRepository : EfCoreRepository<TerminalDbContext, Comment, long>, ICommentRepository
    {
        public CommentRepository(IDbContextProvider<TerminalDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        /// <summary>
        /// 重写获取评论详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<Comment> GetAsync(long id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            var comment = await base.GetAsync(id, includeDetails, cancellationToken);
            if (includeDetails)
            {
                var context = await GetDbContextAsync();
                context.Entry(comment).Reference(x => x.User).Load();
            }
            return comment;
        }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userIds"></param>
        /// <param name="entityType"></param>
        /// <param name="entityId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<int> GetCountAsync(long id, long[]? userIds, string? entityType, long entityId, StatusEnum? status)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .WhereIf(id > 0, x => x.Id == id)
                .WhereIf(userIds != null && userIds.Length > 0, x => userIds!.Contains(x.UserId))
                .WhereIf(!entityType.IsNullOrWhiteSpace(), x => x.EntityType == entityType)
                .WhereIf(entityId > 0, x => x.EntityId == entityId)
                .WhereIf(status != null, x => x.Status == status)
                .CountAsync();
        }
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="id"></param>
        /// <param name="userIds"></param>
        /// <param name="entityType"></param>
        /// <param name="entityId"></param>
        /// <param name="status"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        public async Task<List<Comment>> GetListAsync(int skipCount, int maxResultCount, long id, long[]? userIds, string? entityType, long entityId, StatusEnum? status, bool includeDetails = false)
        {
            var dbSet = await GetDbSetAsync();
            var comments = await dbSet
                .WhereIf(id > 0, x => x.Id == id)
                .WhereIf(userIds != null && userIds.Length > 0, x => userIds!.Contains(x.UserId))
                .WhereIf(!entityType.IsNullOrWhiteSpace(), x => x.EntityType == entityType)
                .WhereIf(entityId > 0, x => x.EntityId == entityId)
                .WhereIf(status != null, x => x.Status == status)
                .OrderByDescending(x => x.Id)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();

            if (includeDetails)
            {
                var context = await GetDbContextAsync();
                comments.ForEach(t =>
                {
                    context.Entry(t).Reference(x => x.User).Load();
                });
            }
            return comments;
        }

        /// <summary>
        /// 删除
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
        /// 获取评论
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="entityId"></param>
        /// <param name="cursor"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        public async Task<List<Comment>> GetCommentsAsync(string entityType, long entityId, long cursor, int limit = 20, bool includeDetails = false)
        {
            var dbSet = await GetDbSetAsync();
            var comments = await dbSet
                .Where(x => x.EntityType == entityType)
                .Where(x => x.EntityId == entityId)
                .Where(x => x.Status == StatusEnum.Normal)
                .WhereIf(cursor > 0, x => x.Id < cursor)
                .OrderByDescending(x => x.Id)
                .Take(limit)
                .ToListAsync();
            if (includeDetails)
            {
                var context = await GetDbContextAsync();
                comments.ForEach(t =>
                {
                    context.Entry(t).Reference(x => x.User).Load();
                });
            }
            return comments;
        }
        /// <summary>
        /// 获取二级回复
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="cursor"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        public async Task<List<Comment>> GetRepliesAsync(long commentId, long cursor, int limit = 20, bool includeDetails = false)
        {
            var dbSet = await GetDbSetAsync();
            var comments = await dbSet
                .Where(x => x.EntityType == EntityTypeEnum.Comment.ToString().ToLower())
                .Where(x => x.EntityId == commentId)
                .Where(x => x.Status == StatusEnum.Normal)
                .WhereIf(cursor > 0, x => x.Id < cursor)
                .OrderByDescending(x => x.Id)
                .Take(limit)
                .ToListAsync();
            if (includeDetails)
            {
                var context = await GetDbContextAsync();
                comments.ForEach(t =>
                {
                    context.Entry(t).Reference(x => x.User).Load();
                });
            }
            return comments;
        }

        /// <summary>
        /// 评论被回复（二级评论）
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public async Task OnCommentAsync(long commentId)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"update t_comment set comment_count = comment_count + 1 where id = {commentId}");
        }

        /// <summary>
        /// 点赞数+1
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public async Task IncrLikeCountAsync(long commentId)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"update t_comment set like_count = like_count + 1 where id = {commentId}");
        }
    }
}
