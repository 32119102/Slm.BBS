using ContIn.Abp.Terminal.Domain.Users;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ContIn.Abp.Terminal.EntityFrameworkCore.Users
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public class OperateLogRepository : EfCoreRepository<TerminalDbContext, OperateLog, long>, IOperateLogRepository
    {
        public OperateLogRepository(IDbContextProvider<TerminalDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
