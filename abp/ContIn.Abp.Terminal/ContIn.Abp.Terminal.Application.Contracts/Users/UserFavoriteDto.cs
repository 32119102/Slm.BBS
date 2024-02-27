namespace ContIn.Abp.Terminal.Application.Contracts.Users
{
    /// <summary>
    /// 收藏
    /// </summary>
    public class UserFavoriteDto
    {
        /// <summary>
        /// 收藏编号
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
    }
}
