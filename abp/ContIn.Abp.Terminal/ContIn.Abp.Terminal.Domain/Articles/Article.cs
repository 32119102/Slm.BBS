using ContIn.Abp.Terminal.Domain.Shared.Enum;
using ContIn.Abp.Terminal.Domain.Users;
using Volo.Abp.Domain.Entities;

namespace ContIn.Abp.Terminal.Domain.Articles
{
    /// <summary>
    /// 文章
    /// </summary>
    public class Article : Entity<long>
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
        /// 标题
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string? Summary { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string? Content { get; set; }
        /// <summary>
        /// 内容类型
        /// </summary>
        public string? ContentType { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public StatusEnum Status { get; set; }
        /// <summary>
        /// 是否是分享的文章，如果是这里只会显示文章摘要，原文需要跳往原链接查看
        /// </summary>
        public bool Share { get; set; }
        /// <summary>
        /// 原文链接
        /// </summary>
        public string? SourceUrl { get; set; }
        /// <summary>
        /// 查看数量
        /// </summary>
        public int ViewCount { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public long UpdateTime { get; set; }

        /// <summary>
        /// 标签信息
        /// </summary>
        public ICollection<ArticleTag>? ArticleTags { get; set; }
    }
}
