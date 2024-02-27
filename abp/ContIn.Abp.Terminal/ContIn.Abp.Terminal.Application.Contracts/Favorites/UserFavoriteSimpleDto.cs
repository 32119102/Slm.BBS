using ContIn.Abp.Terminal.Application.Contracts.Users;

namespace ContIn.Abp.Terminal.Application.Contracts.Favorites
{
    /// <summary>
    /// 收藏
    /// </summary>
    public class UserFavoriteSimpleDto
    {
        /// <summary>
        /// 收藏编号
        /// </summary>
        public long FavoriteId { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserSimpleDto? User { get; set; }

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
        /// 标题
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// 内容，显示简介
        /// </summary>
        public string? Content { get; set; }
        /// <summary>
        /// 实体地址
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool Deleted { get; set; } = false;
    }
}
