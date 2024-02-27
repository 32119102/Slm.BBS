using ContIn.Abp.Terminal.Domain.Shared;

namespace ContIn.Abp.Terminal.Application.LocalEventBus
{
    /// <summary>
    /// 话题被 点赞收藏推荐 事件
    /// </summary>
    public class TopicOperateEvent
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
        /// 类型
        /// </summary>
        public MessageType MessageType { get; set; }
    }
}
