using ContIn.Abp.Terminal.Domain.Shared.Enum;
using ContIn.Abp.Terminal.Domain.Topics;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ContIn.Abp.Terminal.EntityFrameworkCore.Topics
{
    /// <summary>
    /// 话题标签
    /// </summary>
    public class TopicTagRepository : EfCoreRepository<TerminalDbContext, TopicTag, long>, ITopicTagRepository
    {
        public TopicTagRepository(IDbContextProvider<TerminalDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        /// <summary>
        /// 保存话题标签信息
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="tagIds"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public async Task AddTopicTags(long topicId, long[] tagIds, long time)
        {
            foreach (var tag in tagIds)
            {
                await InsertAsync(new TopicTag()
                {
                    TopicId = topicId,
                    TagId = tag,
                    Status = Domain.Shared.Enum.StatusEnum.Normal,
                    LastCommentTime = time,
                    CreateTime = time
                });
            }
        }

        /// <summary>
        /// 获取标签相关文章
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="cursor"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<List<TopicTag>> GetTagTopicsAsync(long tagId, long cursor, int limit = 10)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.Where(x => x.TagId == tagId && x.Status == Domain.Shared.Enum.StatusEnum.Normal)
                .WhereIf(cursor > 0, x => x.Id < cursor)
                .OrderByDescending(x => x.LastCommentTime)
                .Take(limit)
                .ToListAsync();
        }
        /// <summary>
        /// 设置删除状态
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public async Task SetDeleteAsync(long topicId)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"update t_topic_tag set status = {StatusEnum.Delete} where topic_id = {topicId}");
        }
    }
}
