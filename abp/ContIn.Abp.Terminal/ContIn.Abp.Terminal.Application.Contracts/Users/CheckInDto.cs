namespace ContIn.Abp.Terminal.Application.Contracts.Users
{
    /// <summary>
    /// 签到
    /// </summary>
    public class CheckInDto
    {
        /// <summary>
        /// 编号
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserDto? User { get; set; }

        /// <summary>
        /// 最后一次签到
        /// </summary>
        public long DayName { get; set; }
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
        /// <summary>
        /// 今天是否签到
        /// </summary>
        public bool CheckIn { get; set; }
    }
}
