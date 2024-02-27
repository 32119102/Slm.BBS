using ContIn.Abp.Terminal.Domain.Shared.Enum;
using Volo.Abp.Domain.Entities;

namespace ContIn.Abp.Terminal.Domain.Users
{
    /// <summary>
    /// 关注粉丝
    /// </summary>
    public class UserFollow : Entity<long>
    {
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
        public long CreateTime{ get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public User? User { get; set; }
        /// <summary>
        /// 关注用户信息
        /// </summary>
        public User? Other { get; set; }
    }
}