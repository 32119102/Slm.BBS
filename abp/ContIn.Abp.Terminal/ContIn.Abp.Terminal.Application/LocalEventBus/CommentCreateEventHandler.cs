using ContIn.Abp.Terminal.Application.Contracts.Articles;
using ContIn.Abp.Terminal.Application.Contracts.Messages;
using ContIn.Abp.Terminal.Application.Contracts.Topics;
using ContIn.Abp.Terminal.Core.Helpers;
using ContIn.Abp.Terminal.Domain.Comments;
using ContIn.Abp.Terminal.Domain.Shared;
using ContIn.Abp.Terminal.Domain.Shared.Enum;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.Json;

namespace ContIn.Abp.Terminal.Application.LocalEventBus
{
    /// <summary>
    /// 评论创建
    /// </summary>
    public class CommentCreateEventHandler : ILocalEventHandler<CommentCreateEvent>, ITransientDependency
    {
        private readonly ILogger<CommentCreateEventHandler> _logger;
        private readonly IMessageAppService _messageAppService;
        private readonly ITopicAppService _topicAppService;
        private readonly IArticleAppService _articleAppService;
        private readonly ICommentRepository _commentRepository;
        private readonly IJsonSerializer _jsonSerializer;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="messageAppService"></param>
        /// <param name="articleAppService"></param>
        /// <param name="topicAppService"></param>
        /// <param name="commentRepository"></param>
        /// <param name="jsonSerializer"></param>
        public CommentCreateEventHandler(ILogger<CommentCreateEventHandler> logger
            , IMessageAppService messageAppService
            , ITopicAppService topicAppService
            , IArticleAppService articleAppService
            , ICommentRepository commentRepository
            , IJsonSerializer jsonSerializer)
        {
            _logger = logger;
            _messageAppService = messageAppService;
            _topicAppService = topicAppService;
            _articleAppService = articleAppService;
            _commentRepository = commentRepository;
            _jsonSerializer = jsonSerializer;

        }
        /// <summary>
        /// 事件处理
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public async Task HandleEventAsync(CommentCreateEvent eventData)
        {
            try
            {
                long toUserId = 0;
                var title = string.Empty;
                var quoteContent = string.Empty;
                MessageType type = MessageType.TopicComment;
                var extraData = _jsonSerializer.Serialize(new MessageEntityExtraDataDto()
                {
                    EntityType = eventData.EntityType,
                    EntityId = eventData.EntityId,
                    CommentId = eventData.CommentId,
                    QuoteId = eventData.QuoteId
                });
                // 如果评论的是实体，给实体作者发送消息
                // 如果是评论的评论，给评论作者发送消息
                if (eventData.EntityType!.Equals(EntityTypeEnum.Topic.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    var topic = await _topicAppService.GetSimpleAsync(eventData.EntityId);
                    toUserId = topic?.User?.Id ?? 0;
                    title = "回复了你的话题";
                    quoteContent = $"《{topic?.Title}》";
                    type = MessageType.TopicComment;
                }
                else if (eventData.EntityType!.Equals(EntityTypeEnum.Article.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    var article = await _articleAppService.GetSimpleAsync(eventData.EntityId);
                    toUserId = article?.User?.Id ?? 0;
                    title = "回复了你的文章";
                    quoteContent = $"《{article?.Title}》";
                    type = MessageType.ArticleComment;
                }
                else if (eventData.EntityType!.Equals(EntityTypeEnum.Comment.ToString(), StringComparison.OrdinalIgnoreCase))
                { 
                    var comment = await _commentRepository.GetAsync(eventData.EntityId);
                    toUserId = comment?.UserId ?? 0;
                    title = "回复了你的评论";
                    quoteContent = $"《{MarkdigHelper.GetSummary(comment?.Content, Constants.CommentSummaryLength)}》";
                    type = MessageType.CommentReply;
                }

                // 如果是自己给自己评论，不产生消息
                if (toUserId == 0 || toUserId == eventData.UserId)
                {
                    return;
                }

                await _messageAppService.AddMessageAsync(eventData.UserId, toUserId, title, eventData.SummaryCommentContent ?? string.Empty, quoteContent, type, extraData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
