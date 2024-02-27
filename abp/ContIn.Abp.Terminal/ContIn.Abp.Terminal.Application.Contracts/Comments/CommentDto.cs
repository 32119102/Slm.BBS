using ContIn.Abp.Terminal.Application.Contracts.Users;
using ContIn.Abp.Terminal.Domain.Shared.Enum;

namespace ContIn.Abp.Terminal.Application.Contracts.Comments
{
    /// <summary>
    /// 评论
    /// </summary>
    public class CommentDto
    {
        /// <summary>
        /// 评论编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserDto? User { get; set; }

        /// <summary>
        /// 被评论实体类型
        /// </summary>
        public string? EntityType { get; set; }
        /// <summary>
        /// 被评论实体编号
        /// </summary>
        public long EntityId { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string? Content { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string? ImageList { get; set; }
        /// <summary>
        /// 内容类型
        /// </summary>
        public string? ContentType { get; set; }
        /// <summary>
        /// 引用的评论编号
        /// </summary>
        public long QuoteId { get; set; }
        /// <summary>
        /// 点赞数量
        /// </summary>
        public int LikeCount { get; set; }
        /// <summary>
        /// 评论数量
        /// </summary>
        public int CommentCount { get; set; }
        /// <summary>
        /// UserAgent
        /// </summary>
        public string? UserAgent { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        public string? IP { get; set; }
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
