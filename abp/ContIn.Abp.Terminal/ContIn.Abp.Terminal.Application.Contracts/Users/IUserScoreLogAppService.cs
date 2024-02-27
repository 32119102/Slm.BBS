using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ContIn.Abp.Terminal.Application.Contracts.Users
{
    /// <summary>
    /// 用户积分记录
    /// </summary>
    public interface IUserScoreLogAppService : IApplicationService
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<UserScoreLogDto>> PostPagedAsync(SearchUserScoreLogPagedDto input);

        /// <summary>
        /// 获取用户积分记录
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<SitePagingResultDto<UserScoreLogDto>> GetScoreLogsAsync(int page = 1);
    }
}
