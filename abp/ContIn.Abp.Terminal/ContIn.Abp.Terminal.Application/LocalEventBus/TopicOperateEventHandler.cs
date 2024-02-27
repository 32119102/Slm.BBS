using ContIn.Abp.Terminal.Application.Contracts.Messages;
using ContIn.Abp.Terminal.Application.Contracts.Topics;
using ContIn.Abp.Terminal.Domain.Shared;
using ContIn.Abp.Terminal.Domain.Shared.Enum;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.Json;

namespace ContIn.Abp.Terminal.Application.LocalEventBus
{
    /// <summary>
    /// 话题操作事件
    /// </summary>
    public class TopicOperateEventHandler : ILocalEventHandler<TopicOperateEvent>, ITransientDependency
    {
        private readonly ILogger<TopicOperateEventHandler> _logger;
        private readonly ITopicAppService _topicAppService;
        private readonly IMessageAppService _messageAppService;
        private readonly IJsonSerializer _jsonSerializer;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="topicAppService"></param>
        /// <param name="messageAppService"></param>
        /// <param name="jsonSerializer"></param>
        public TopicOperateEventHandler(ILogger<TopicOperateEventHandler> logger
            , ITopicAppService topicAppService
            , IMessageAppService messageAppService
            , IJsonSerializer jsonSerializer)
        { 
            _logger = logger;
            _topicAppService = topicAppService;
            _messageAppService = messageAppService;
            _jsonSerializer = jsonSerializer;
        }
        /// <summary>
        /// 事件处理程序
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public async Task HandleEventAsync(TopicOperateEvent eventData)
        {
            try
            {
                long toUserId = 0;
                string title = string.Empty;
                var quoteContent = string.Empty;

                switch (eventData.MessageType)
                {
                    case MessageType.TopicLike:
                        title = "点赞了你的话题";
                        break;
                    case MessageType.TopicFavorite:
                        title = "收藏了你的话题";
                        break;
                    case MessageType.TopicRecommend:
                        title = "你的话题被设为推荐";
                        break;
                    case MessageType.TopicDelete:
                        title = "你的话题被删除";
                        break;
                }

                if (eventData.EntityType!.Equals(EntityTypeEnum.Topic.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    var topic = await _topicAppService.GetSimpleAsync(eventData.EntityId);
                    toUserId = topic?.User?.Id ?? 0;
                    quoteContent = $"《{topic?.Title}》";
                }
                var extraData = _jsonSerializer.Serialize(new MessageTopicExtraDataDto()
                {
                    TopicId = eventData.EntityId,
                    UserId = eventData.UserId
                });

                await _messageAppService.AddMessageAsync(eventData.UserId, toUserId, title, string.Empty, quoteContent, eventData.MessageType, extraData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
