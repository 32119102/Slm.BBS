using ContIn.Abp.Terminal.Domain.Shared.Enum;

namespace ContIn.Abp.Terminal.Application.Contracts.Users
{
    /// <summary>
    /// 用户积分记录
    /// </summary>
    public class UserScoreLogDto
    {
        /// <summary>
        /// 记录编号
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 积分来源类型
        /// </summary>
        public string? SourceType { get; set; }
        /// <summary>
        /// 积分来源编号
        /// </summary>
        public string? SourceId { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public ScoreTypeEnum Type { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public long Score { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTime { get; set; }
    }
}
