using ContIn.Abp.Terminal.Domain.Shared.Enum;
using ContIn.Abp.Terminal.Domain.Tags;
using Volo.Abp.Domain.Entities;

namespace ContIn.Abp.Terminal.Domain.Articles
{
    /// <summary>
    /// 文章标签
    /// </summary>
    public class ArticleTag : Entity<long>
    {
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
        public Tag? Tag { get; set; }

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
