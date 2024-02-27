using ContIn.Abp.Terminal.Domain.Shared.Enum;
using ContIn.Abp.Terminal.Domain.Tags;
using Volo.Abp.Domain.Entities;

namespace ContIn.Abp.Terminal.Domain.Topics
{
    /// <summary>
    /// 话题标签
    /// </summary>
    public class TopicTag : Entity<long>
    {
        /// <summary>
        /// 话题编号
        /// </summary>
        public long TopicId { get; set; }
        /// <summary>
        /// 标签编号
        /// </summary>
        public long TagId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public StatusEnum Status { get; set; }
        /// <summary>
        /// 最后回复时间
        /// </summary>
        public long LastCommentTime { get; set; }
        /// <summary>
        /// 最后回复用户编号
        /// </summary>
        public long LastCommentUserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTime { get; set; }

        /// <summary>
        /// 标签信息
        /// </summary>
        public Tag? Tag { get; set; }
    }
}
