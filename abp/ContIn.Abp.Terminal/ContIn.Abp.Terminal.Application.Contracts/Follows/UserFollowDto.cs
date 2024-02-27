using ContIn.Abp.Terminal.Domain.Shared.Enum;

namespace ContIn.Abp.Terminal.Application.Contracts.Follows
{
    /// <summary>
    /// 粉丝关注
    /// </summary>
    public class UserFollowDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 关注用户编号
        /// </summary>
        public long OtherId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public StatusEnum Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTime { get; set; }
    }
}
