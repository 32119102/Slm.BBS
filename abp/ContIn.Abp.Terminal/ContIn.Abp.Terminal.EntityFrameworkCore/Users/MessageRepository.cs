using ContIn.Abp.Terminal.Domain.Shared;
using ContIn.Abp.Terminal.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ContIn.Abp.Terminal.EntityFrameworkCore.Users
{
    /// <summary>
    /// 消息仓储实现
    /// </summary>
    public class MessageRepository : EfCoreRepository<TerminalDbContext, Message, long>, IMessageRepository
    {
        public MessageRepository(IDbContextProvider<TerminalDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        /// <summary>
        /// 获取用户消息列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        public async Task<(int, List<Message>)> GetMessagesAsync(long userId, int page = 1, int limit = 10, bool includeDetails = false)
        {
            var dbSet = await GetDbSetAsync();
            var count = await dbSet
                .Where(x => x.UserId == userId)
                .CountAsync();
            var messages = await dbSet
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
            if (includeDetails)
            {

            }
            return (count, messages);
        }

        /// <summary>
        /// 获取用户最近未读消息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        public async Task<List<Message>> GetMsgRecentAsync(long userId, int limit = 3, bool includeDetails = false)
        {
            var dbSet = await GetDbSetAsync();
            var messages = await dbSet
                .Where(x => x.UserId == userId)
                .Where(x => x.Status == MessageStatus.UnRead)
                .OrderByDescending(x => x.Id)
                .Take(limit)
                .ToListAsync();
            if (includeDetails)
            { 
                
            }
            return messages;
        }

        /// <summary>
        /// 获取未读消息数量
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> GetUnReadCountAsync(long userId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.Where(x => x.UserId == userId).Where(x => x.Status == MessageStatus.UnRead).CountAsync();
        }
        /// <summary>
        /// 标记所有未读消息为已读
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task MarkReadAsync(long userId)
        {
            var context = await GetDbContextAsync();
            await context.Database
                .ExecuteSqlInterpolatedAsync($"update t_message set status = {MessageStatus.HaveRead} where user_id = {userId} and status = {MessageStatus.UnRead}");
        }
    }
}
