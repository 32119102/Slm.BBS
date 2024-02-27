using ContIn.Abp.Terminal.Domain.Shared.Enum;
using System.ComponentModel.DataAnnotations;

namespace ContIn.Abp.Terminal.Application.Contracts.Topics
{
    /// <summary>
    /// 节点
    /// </summary>
    public class CreateUpdateTopicNodeDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "节点名称不能为空")]
        public string? Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SortNo { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public StatusEnum Status { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string? Logo { get; set; }
    }
}
