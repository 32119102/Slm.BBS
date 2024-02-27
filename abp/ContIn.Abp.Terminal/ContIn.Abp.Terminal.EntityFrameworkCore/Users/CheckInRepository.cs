using ContIn.Abp.Terminal.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ContIn.Abp.Terminal.EntityFrameworkCore.Users
{
    /// <summary>
    /// 签到
    /// </summary>
    public class CheckInRepository : EfCoreRepository<TerminalDbContext, CheckIn, long>, ICheckInRepository
    {
        public CheckInRepository(IDbContextProvider<TerminalDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        /// <summary>
        /// 获取用户签到信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<CheckIn?> GetCheckInAsync(long userId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.Where(x => x.UserId == userId).FirstOrDefaultAsync();
        }
        /// <summary>
        /// 获取当天签到排行榜，越早签到的排在最前面
        /// </summary>
        /// <param name="dayName"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        public async Task<List<CheckIn>?> GetRankAsync(long dayName, int limit = 10, bool includeDetails = true)
        {
            var dbSet = await GetDbSetAsync();
            var dbContext = await GetDbContextAsync();
            var list = await dbSet
                .Where(x => x.LatestDayName == dayName)
                .OrderBy(x => x.UpdateTime)
                .Take(limit)
                .ToListAsync();
            if (includeDetails)
            {
                list.ForEach(x =>
                {
                    dbContext.Entry(x).Reference(m => m.User).Load();
                });
            }
            return list;
        }
    }
}
