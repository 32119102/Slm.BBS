using Volo.Abp.Domain.Repositories;

namespace ContIn.Abp.Terminal.Domain.Users
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public interface IOperateLogRepository : IBasicRepository<OperateLog, long>
    {
    }
}
