using Volo.Abp.Domain.Entities;

namespace ContIn.Abp.Terminal.Domain.Feed
{
    /// <summary>
    /// 用户Feed流
    /// </summary>
    public class UserFeed : Entity<long>
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 实体编号
        /// </summary>
        public long DataId { get; set; }
        /// <summary>
        /// 实体类型
        /// </summary>
        public string? DataType { get; set; }

        /// <summary>
        /// 实体作者ID
        /// </summary>
        public long AuthorId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTime { get; set; }
    }
}
