using Volo.Abp.Domain.Entities;

namespace ContIn.Abp.Terminal.Domain.Users
{
    /// <summary>
    /// 签到
    /// </summary>
    public class CheckIn : Entity<long>
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public User? User { get; set; }

        /// <summary>
        /// 最后一次签到
        /// </summary>
        public long LatestDayName { get; set; }
        /// <summary>
        /// 连续签到天数
        /// </summary>
        public long ConsecutiveDays { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public long UpdateTime { get; set; }
    }
}
