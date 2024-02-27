using ContIn.Abp.Terminal.Application.Contracts;
using ContIn.Abp.Terminal.Application.Contracts.Users;
using ContIn.Abp.Terminal.Domain.Shared;
using ContIn.Abp.Terminal.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ContIn.Abp.Terminal.Application.Users
{
    /// <summary>
    /// 用户积分记录
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = SwaggerGroupName.User)]
    public class UserScoreLogAppService : ApplicationService, IUserScoreLogAppService
    {
        private readonly IUserScoreLogRepository _repository;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public UserScoreLogAppService(IUserScoreLogRepository repository)
        { 
            _repository = repository;
        }

        /// <summary>
        /// 获取用户积分记录
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<SitePagingResultDto<UserScoreLogDto>> GetScoreLogsAsync(int page = 1)
        {
            int limit = 10;
            page = page < 1 ? 1 : page;
            var userId = CurrentUser.GetUserId();
            var scoreLogs = await _repository.GetScoreLogsAsync(userId, page, limit);
            return new SitePagingResultDto<UserScoreLogDto>()
            {
                Page = new SitePagingPage()
                {
                    Limit = limit,
                    Page = page,
                    Total = scoreLogs.Item1
                },
                Results = ObjectMapper.Map<List<UserScoreLog>, List<UserScoreLogDto>>(scoreLogs.Item2)
            };
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<UserScoreLogDto>> PostPagedAsync(SearchUserScoreLogPagedDto input)
        {
            var logs = await _repository.GetListAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.UserId
            );
            var totalCount = await _repository.GetCountAsync(input.UserId);
            return new PagedResultDto<UserScoreLogDto>(
                totalCount,
                ObjectMapper.Map<List<UserScoreLog>, List<UserScoreLogDto>>(logs)
            );
        }
    }
}
