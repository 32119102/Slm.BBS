namespace ContIn.Abp.Terminal.Application.Contracts.Messages
{
    /// <summary>
    /// 消息实体相关扩展字段
    /// </summary>
    public class MessageEntityExtraDataDto
    {
        /// <summary>
        /// 实体类型
        /// </summary>
        public string? EntityType { get; set; }
        /// <summary>
        /// 实体编号
        /// </summary>
        public long EntityId { get; set; }
        /// <summary>
        /// 评论编号
        /// </summary>
        public long CommentId { get; set; }
        /// <summary>
        /// 引用编号
        /// </summary>
        public long QuoteId { get; set; }
    }
}
