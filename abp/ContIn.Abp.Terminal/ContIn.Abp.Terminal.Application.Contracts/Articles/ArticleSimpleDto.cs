using ContIn.Abp.Terminal.Application.Contracts.Tags;
using ContIn.Abp.Terminal.Application.Contracts.Users;
using ContIn.Abp.Terminal.Domain.Shared.Enum;

namespace ContIn.Abp.Terminal.Application.Contracts.Articles
{
    /// <summary>
    /// 文章简要模型
    /// </summary>
    public class ArticleSimpleDto
    {
        /// <summary>
        /// 文章编号
        /// </summary>
        public long ArticleId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTime { get; set; }
        /// <summary>
        /// 原文链接
        /// </summary>
        public string? SourceUrl { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public StatusEnum Status { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string? Summary { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// 浏览数量
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserSimpleDto? User { get; set; }
        /// <summary>
        /// 标签信息
        /// </summary>
        public List<TagSimpleDto>? Tags { get; set; }
    }
}
