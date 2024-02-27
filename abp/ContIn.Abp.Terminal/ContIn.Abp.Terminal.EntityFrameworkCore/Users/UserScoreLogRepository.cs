using ContIn.Abp.Terminal.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ContIn.Abp.Terminal.EntityFrameworkCore.Users
{
    /// <summary>
    /// 用户积分记录
    /// </summary>
    public class UserScoreLogRepository : EfCoreRepository<TerminalDbContext, UserScoreLog, long>, IUserScoreLogRepository
    {
        public UserScoreLogRepository(IDbContextProvider<TerminalDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> GetCountAsync(long userId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.CountAsync(x => x.UserId == userId);
        }
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<UserScoreLog>> GetListAsync(int skipCount, int maxResultCount, long userId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Id)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }
        /// <summary>
        /// 获取用户积分记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<(int, List<UserScoreLog>)> GetScoreLogsAsync(long userId, int page = 1, int limit = 20)
        {
            var dbSet = await GetDbSetAsync();
            var count = await dbSet.Where(x => x.UserId == userId).CountAsync();
            var list = await dbSet.Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
            return (count, list);
        }
    }
}
