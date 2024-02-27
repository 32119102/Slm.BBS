namespace ContIn.Abp.Terminal.Application.LocalEventBus
{
    /// <summary>
    /// 评论被创建
    /// </summary>
    public class CommentCreateEvent
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 实体类型
        /// </summary>
        public string? EntityType { get; set; }
        /// <summary>
        /// 实体ID
        /// </summary>
        public long EntityId { get; set; }

        /// <summary>
        /// 评论ID
        /// </summary>
        public long CommentId { get; set; }
        /// <summary>
        /// 引用ID
        /// </summary>
        public long QuoteId { get; set; }

        /// <summary>
        /// 评论内容简介
        /// </summary>
        public string? SummaryCommentContent { get; set; }
    }
}
