using ContIn.Abp.Terminal.Domain.Shared.Enum;

namespace ContIn.Abp.Terminal.Application.LocalEventBus
{
    /// <summary>
    /// 用户积分变化事件对象
    /// </summary>
    public class UserScoreChangedEvent
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 实体类型
        /// </summary>
        public EntityTypeEnum SourceType { get; set; }
        /// <summary>
        /// 实体ID
        /// </summary>
        public long SourceId { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public ScoreTypeEnum Type { get; set; }
    }
}
