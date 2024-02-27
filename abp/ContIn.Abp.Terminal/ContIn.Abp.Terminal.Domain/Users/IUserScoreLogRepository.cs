using Volo.Abp.Domain.Repositories;

namespace ContIn.Abp.Terminal.Domain.Users
{
    /// <summary>
    /// 用户积分记录
    /// </summary>
    public interface IUserScoreLogRepository : IBasicRepository<UserScoreLog, long>
    {
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<UserScoreLog>> GetListAsync(int skipCount, int maxResultCount, long userId);
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> GetCountAsync(long userId);
        /// <summary>
        /// 获取用户积分记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<(int, List<UserScoreLog>)> GetScoreLogsAsync(long userId, int page = 1, int limit = 20);
    }
}
