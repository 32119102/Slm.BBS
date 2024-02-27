using Volo.Abp.Application.Services;

namespace ContIn.Abp.Terminal.Application.Contracts.Users
{
    /// <summary>
    /// 签到
    /// </summary>
    public interface ICheckInAppService : IApplicationService
    {
        /// <summary>
        /// 获取用户签到信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<CheckInDto?> GetCheckInAsync(long userId);
        /// <summary>
        /// 用户签到
        /// </summary>
        /// <returns></returns>
        Task PostCheckInAsync();
        /// <summary>
        /// 获取当前签到排行榜，越早签到的排在越前面
        /// </summary>
        /// <returns></returns>
        Task<List<CheckInDto>?> GetRankAsync();
    }
}
