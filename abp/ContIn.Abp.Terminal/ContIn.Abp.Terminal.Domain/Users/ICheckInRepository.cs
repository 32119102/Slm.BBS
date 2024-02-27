using Volo.Abp.Domain.Repositories;

namespace ContIn.Abp.Terminal.Domain.Users
{
    /// <summary>
    /// 签到
    /// </summary>
    public interface ICheckInRepository : IBasicRepository<CheckIn, long>
    {
        /// <summary>
        /// 获取用户签到信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<CheckIn?> GetCheckInAsync(long userId);
        /// <summary>
        /// 获取当天签到排行榜，越早签到的排在最前面
        /// </summary>
        /// <param name="dayName"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        Task<List<CheckIn>?> GetRankAsync(long dayName, int limit = 10, bool includeDetails = true);
    }
}
