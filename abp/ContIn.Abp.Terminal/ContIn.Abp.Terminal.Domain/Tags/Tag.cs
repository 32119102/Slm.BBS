using ContIn.Abp.Terminal.Domain.Shared.Enum;
using Volo.Abp.Domain.Entities;

namespace ContIn.Abp.Terminal.Domain.Tags
{
    /// <summary>
    /// 标签
    /// </summary>
    public class Tag : Entity<long>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string? Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public virtual string? Description { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual StatusEnum Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual long CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual long UpdateTime { get; set; }
    }
}
