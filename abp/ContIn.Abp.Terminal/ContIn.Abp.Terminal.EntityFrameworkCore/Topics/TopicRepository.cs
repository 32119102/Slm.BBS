using ContIn.Abp.Terminal.Domain.Shared.Enum;
using ContIn.Abp.Terminal.Domain.Topics;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ContIn.Abp.Terminal.EntityFrameworkCore.Topics
{
    /// <summary>
    /// 话题
    /// </summary>
    public class TopicRepository : EfCoreRepository<TerminalDbContext, Topic, long>, ITopicRepository
    {
        public TopicRepository(IDbContextProvider<TerminalDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        /// <summary>
        /// 重写获取话题详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<Topic> GetAsync(long id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            var topic = await base.GetAsync(id, includeDetails, cancellationToken);
            if (includeDetails)
            {
                var context = await GetDbContextAsync();
                context.Entry(topic).Reference(x => x.Node).Load();
                context.Entry(topic).Reference(x => x.User).Load();
                context.Entry(topic).Collection(x => x.TopicTags!).Load();

                topic.TopicTags?.ToList().ForEach(m =>
                {
                    context.Entry(m).Reference(x => x.Tag).Load();
                });
            }
            return topic;
        }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="title"></param>
        /// <param name="recommend">是否推荐</param>
        /// <param name="sticky"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<int> GetCountAsync(long[]? userIds, string? title, bool? recommend, bool? sticky, StatusEnum? status)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .WhereIf(userIds != null && userIds.Length > 0, x => userIds!.Contains(x.UserId))
                .WhereIf(!title.IsNullOrWhiteSpace(), x => x.Title!.Contains(title!))
                .WhereIf(recommend != null, x => x.Recommend == recommend)
                .WhereIf(sticky != null, x => x.Sticky == sticky)
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
        /// <param name="recommend"></param>
        /// <param name="sticky"></param>
        /// <param name="status"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        public async Task<List<Topic>> GetListAsync(int skipCount, int maxResultCount, long[]? userIds, string? title, bool? recommend, bool? sticky, StatusEnum? status, bool includeDetails = false)
        {
            var dbSet = await GetDbSetAsync();
            var topics = await dbSet
                .WhereIf(userIds != null && userIds.Length > 0, x => userIds!.Contains(x.UserId))
                .WhereIf(!title.IsNullOrWhiteSpace(), x => x.Title!.Contains(title!))
                .WhereIf(recommend != null, x => x.Recommend == recommend)
                .WhereIf(sticky != null, x => x.Sticky == sticky)
                .WhereIf(status != null, x => x.Status == status)
                .OrderByDescending(x => x.Id)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();

            if (includeDetails)
            {
                var context = await GetDbContextAsync();
                topics.ForEach(t =>
                {
                    context.Entry(t).Reference(x => x.Node).Load();
                    context.Entry(t).Reference(x => x.User).Load();
                    context.Entry(t).Collection(x => x.TopicTags!).Load();

                    t.TopicTags?.ToList().ForEach(m =>
                    {
                        context.Entry(m).Reference(x => x.Tag).Load();
                    });
                });
            }
            return topics;
        }

        /// <summary>
        /// 获取话题列表
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="recommend"></param>
        /// <param name="cursor"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        public async Task<List<Topic>> GetPagedTopicsAsync(long nodeId, bool recommend, long cursor, int limit = 20, bool includeDetails = false)
        {
            var dbSet = await GetDbSetAsync();
            var topics = await dbSet
                .WhereIf(nodeId > 0, x => x.NodeId == nodeId)
                .WhereIf(recommend, x => x.Recommend == true)
                .WhereIf(cursor > 0, x => x.LastCommentTime < cursor)
                .Where(x => x.Status == StatusEnum.Normal)
                .OrderByDescending(x => x.LastCommentTime)
                .Take(limit)
                .ToListAsync();

            await IncludeDetails(topics, includeDetails);
            return topics;
        }

        /// <summary>
        /// 获取置顶话题
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        public async Task<List<Topic>> GetStickyTopicsAsync(long nodeId, int limit = 3, bool includeDetails = false)
        {
            var dbSet = await GetDbSetAsync();
            var topics = await dbSet
                .WhereIf(nodeId > 0, x => x.NodeId == nodeId)
                .Where(x => x.Sticky == true)
                .Where(x => x.Status == StatusEnum.Normal)
                .OrderByDescending(x => x.StickyTime)
                .Take(limit)
                .ToListAsync();
            await IncludeDetails(topics, includeDetails);
            return topics;
        }

        /// <summary>
        /// 获取用户话题
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cursor"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        public async Task<List<Topic>> GetUserTopicsAsync(long userId, long cursor, int limit = 20, bool includeDetails = false)
        {
            var dbSet = await GetDbSetAsync();
            var topics = await dbSet
                .Where(x => x.UserId == userId)
                .Where(x => x.Status == StatusEnum.Normal)
                .WhereIf(cursor > 0, x => x.Id < cursor)
                .OrderByDescending(x => x.Id)
                .Take(limit)
                .ToListAsync();

            await IncludeDetails(topics, includeDetails);
            return topics;
        }

        /// <summary>
        /// 增加点赞数量
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task IncrLikeCountAsync(long id)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"update t_topic set like_count = like_count + 1 where id = {id}");
        }

        /// <summary>
        /// 增加浏览量
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task IncrViewCount(long id)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"update t_topic set view_count = view_count + 1 where id = {id}");
        }

        /// <summary>
        /// 当帖子被评论的时候，更新最后回复时间、回复数量+1
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="userId"></param>
        /// <param name="commentCreateTime"></param>
        /// <returns></returns>
        public async Task OnCommentAsync(long topicId, long userId, long commentCreateTime)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"update t_topic set last_comment_time={commentCreateTime}, last_comment_user_id={userId},comment_count=comment_count+1 where id = {topicId}");
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"update t_topic_tag set last_comment_time={commentCreateTime}, last_comment_user_id={userId} where topic_id = {topicId}");
        }

        /// <summary>
        /// 搜索话题
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="nodeId"></param>
        /// <param name="recommend"></param>
        /// <param name="minCreateTime"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        public async Task<(int, List<Topic>)> SearchTopicsAsync(string keyword, long nodeId, bool recommend, long minCreateTime, int page, int limit, bool includeDetails = false)
        {
            var dbSet = await GetDbSetAsync();
            var search = dbSet.Where(x => x.Title!.Contains(keyword))
                .WhereIf(nodeId > 0, x => x.NodeId == nodeId)
                .WhereIf(recommend, x => x.Recommend == true)
                .WhereIf(minCreateTime > 0, x => x.CreateTime > minCreateTime);
            var count = await search.CountAsync();
            var topics = await search.OrderByDescending(x => x.Id)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
            await IncludeDetails(topics, includeDetails);
            return (count, topics);
        }

        /// <summary>
        /// 设置话题置顶
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="sticky"></param>
        /// <param name="stickyTime"></param>
        /// <returns></returns>
        public async Task SetStickyAsync(long topicId, bool sticky, long stickyTime)
        {
            var dbSet = await GetDbSetAsync();
            var topic = await dbSet.Where(x => x.Id == topicId).SingleOrDefaultAsync();
            if (topic != null && topic.Id == topicId)
            {
                topic.Sticky = sticky;
                topic.StickyTime = stickyTime;

                await UpdateAsync(topic);
            }
        }

        /// <summary>
        /// 填充导航属性
        /// </summary>
        /// <param name="topics"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        private async Task IncludeDetails(List<Topic> topics, bool includeDetails = false)
        {
            if (includeDetails)
            {
                var context = await GetDbContextAsync();
                topics.ForEach(t =>
                {
                    context.Entry(t).Reference(x => x.Node).Load();
                    context.Entry(t).Reference(x => x.User).Load();
                    context.Entry(t).Collection(x => x.TopicTags!).Load();
                    t.TopicTags?.ToList().ForEach(m =>
                    {
                        context.Entry(m).Reference(x => x.Tag).Load();
                    });
                });
            }
        }
    }
}
