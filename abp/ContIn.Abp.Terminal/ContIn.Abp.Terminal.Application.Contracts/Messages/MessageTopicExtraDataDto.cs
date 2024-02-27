namespace ContIn.Abp.Terminal.Application.Contracts.Messages
{
    /// <summary>
    /// 消息中话题扩展数据
    /// </summary>
    public class MessageTopicExtraDataDto
    {
        /// <summary>
        /// 话题编号
        /// </summary>
        public long TopicId { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }
    }
}
