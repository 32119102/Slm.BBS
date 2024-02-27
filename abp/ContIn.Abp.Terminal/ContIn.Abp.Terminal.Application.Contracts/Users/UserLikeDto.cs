namespace ContIn.Abp.Terminal.Application.Contracts.Users
{
    /// <summary>
    /// 点赞
    /// </summary>
    public class UserLikeDto
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
        /// 实体类型
        /// </summary>
        public string? EntityType { get; set; }
        /// <summary>
        /// 实体编号
        /// </summary>
        public long EntityId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTime { get; set; }
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserDto? User { get; set; }
    }
}
