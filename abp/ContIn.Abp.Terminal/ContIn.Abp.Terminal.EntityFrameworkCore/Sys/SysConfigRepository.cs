using ContIn.Abp.Terminal.Domain.Sys;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ContIn.Abp.Terminal.EntityFrameworkCore.Sys
{
    public class SysConfigRepository : EfCoreRepository<TerminalDbContext, SysConfig, long>, ISysConfigRepository
    {
        public SysConfigRepository(IDbContextProvider<TerminalDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        /// <summary>
        /// 根据键获取信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<SysConfig?> GetByKeyAsync(string key)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(x => x.Key == key);
        }
    }
}
