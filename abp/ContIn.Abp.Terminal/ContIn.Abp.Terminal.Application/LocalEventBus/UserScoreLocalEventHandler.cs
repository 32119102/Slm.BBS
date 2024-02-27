using ContIn.Abp.Terminal.Application.Contracts.Users;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace ContIn.Abp.Terminal.Application.LocalEventBus
{
    /// <summary>
    /// 用户积分事件处理程序
    /// </summary>
    public class UserScoreLocalEventHandler : ILocalEventHandler<UserScoreChangedEvent>, ITransientDependency
    {
        private readonly ILogger _logger;
        private readonly IUserAppService _userAppService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="userAppService"></param>
        public UserScoreLocalEventHandler(ILogger<UserScoreLocalEventHandler> logger, IUserAppService userAppService)
        {
            _logger = logger;
            _userAppService = userAppService;
        }
        /// <summary>
        /// 处理方法
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public async Task HandleEventAsync(UserScoreChangedEvent eventData)
        {
            try
            {
                await _userAppService.AddScoreAsync(eventData.UserId, eventData.Score, eventData.SourceType, eventData.SourceId, eventData.Description ?? string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
