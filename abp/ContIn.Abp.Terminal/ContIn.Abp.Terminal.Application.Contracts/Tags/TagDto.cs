using ContIn.Abp.Terminal.Domain.Shared.Enum;
using Volo.Abp.Application.Dtos;

namespace ContIn.Abp.Terminal.Application.Contracts.Tags
{
    /// <summary>
    /// 标签数据传输对象
    /// </summary>
    public class TagDto : EntityDto<long>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public StatusEnum Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public long UpdateTime { get; set; }
    }
}
