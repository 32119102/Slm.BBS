using Volo.Abp.Domain.Repositories;

namespace ContIn.Abp.Terminal.Domain.Users
{
    /// <summary>
    /// 第三方账号
    /// </summary>
    public interface IThirdAccountRepository : IBasicRepository<ThirdAccount, long>
    {
    }
}
