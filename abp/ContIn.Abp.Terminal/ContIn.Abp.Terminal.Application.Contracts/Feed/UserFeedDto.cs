namespace ContIn.Abp.Terminal.Application.Contracts.Feed
{
    /// <summary>
    /// Feed
    /// </summary>
    public class UserFeedDto
    {
        /// <summary>
        /// 编号 
        /// </summary>
        public long Id { get; set; }
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
