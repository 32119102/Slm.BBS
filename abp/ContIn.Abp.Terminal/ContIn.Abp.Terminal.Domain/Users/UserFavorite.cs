using Volo.Abp.Domain.Entities;

namespace ContIn.Abp.Terminal.Domain.Users
{
    /// <summary>
    /// 收藏
    /// </summary>
    public class UserFavorite : Entity<long>
    {
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
