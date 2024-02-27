using ContIn.Abp.Terminal.Application.Contracts.Users;
using ContIn.Abp.Terminal.Domain.Shared;

namespace ContIn.Abp.Terminal.Application.Contracts.Messages
{
    /// <summary>
    /// 消息
    /// </summary>
    public class MessageDto
    {
        /// <summary>
        /// 消息编号
        /// </summary>
        public long MessageId { get; set; }
        /// <summary>
        /// 消息发送人
        /// </summary>
        public long FromId { get; set; }
        /// <summary>
        /// 用户编号、消息接收人
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string? Content { get; set; }
        /// <summary>
        /// 引用内容
        /// </summary>
        public string? QuoteContent { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType Type { get; set; }
        /// <summary>
        /// 扩展数据
        /// 话题相关：{ "topicId":1, "userId":1 }
        /// 评论相关：{ "entityType": "", "entityId": "", "commentId": "", "quoteId": "" }
        /// </summary>
        public string? ExtraData { get; set; }
        /// <summary>
        /// 消息状态
        /// </summary>
        public MessageStatus Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTime { get; set; }

        /// <summary>
        /// 详情链接
        /// </summary>
        public string? DetailUrl { get; set; }
        /// <summary>
        /// 消息发送人信息
        /// </summary>
        public UserDto? From { get; set; }
    }
}
