using ContIn.Abp.Terminal.Domain.Shared.Enum;

namespace ContIn.Abp.Terminal.Application.Contracts.Links
{
    /// <summary>
    /// 友情链接
    /// </summary>
    public class FriendlyLinkDto
    {
        /// <summary>
        /// 编号
        /// </summary>
        public long Id { get; set; }
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
