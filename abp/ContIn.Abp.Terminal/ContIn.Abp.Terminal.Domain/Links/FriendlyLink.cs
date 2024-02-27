using ContIn.Abp.Terminal.Domain.Shared.Enum;
using Volo.Abp.Domain.Entities;

namespace ContIn.Abp.Terminal.Domain.Links
{
    /// <summary>
    /// 友情链接
    /// </summary>
    public class FriendlyLink : Entity<long>
    {
        /// <summary>
        /// 链接地址
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Summary { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string? Logo { get; set; }

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
