using ContIn.Abp.Terminal.Domain.Shared.Enum;
using System.ComponentModel.DataAnnotations;

namespace ContIn.Abp.Terminal.Application.Contracts.Links
{
    /// <summary>
    /// 友情链接
    /// </summary>
    public class CreateUpdateFriendlyLinkDto
    {
        /// <summary>
        /// 链接地址
        /// </summary>
        [Required(ErrorMessage = "链接地址不能为空")]
        public string? Url { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "标题不能为空")]
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
    }
}
