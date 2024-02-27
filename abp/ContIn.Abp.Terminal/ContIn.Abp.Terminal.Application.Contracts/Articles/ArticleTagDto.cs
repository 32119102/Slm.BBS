using ContIn.Abp.Terminal.Application.Contracts.Tags;
using ContIn.Abp.Terminal.Domain.Shared.Enum;

namespace ContIn.Abp.Terminal.Application.Contracts.Articles
{
    /// <summary>
    /// 文章标签
    /// </summary>
    public class ArticleTagDto
    {
        /// <summary>
        /// 编号
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 文章编号
        /// </summary>
        public long ArticleId { get; set; }
        /// <summary>
        /// 标签编号
        /// </summary>
        public long TagId { get; set; }

        /// <summary>
        /// 标签信息
        /// </summary>
        public TagDto? Tag { get; set; }

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
