using ContIn.Abp.Terminal.Domain.Users;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ContIn.Abp.Terminal.EntityFrameworkCore.Users
{
    /// <summary>
    /// 第三方账号
    /// </summary>
    public class ThirdAccountRepository : EfCoreRepository<TerminalDbContext, ThirdAccount, long>, IThirdAccountRepository
    {
        public ThirdAccountRepository(IDbContextProvider<TerminalDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
