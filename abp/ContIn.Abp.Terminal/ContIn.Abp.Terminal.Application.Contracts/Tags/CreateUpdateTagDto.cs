using ContIn.Abp.Terminal.Domain.Shared.Enum;
using System.ComponentModel.DataAnnotations;

namespace ContIn.Abp.Terminal.Application.Contracts.Tags
{
    /// <summary>
    /// 新增更新数据传输对象
    /// </summary>
    public class CreateUpdateTagDto : IValidatableObject
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "标签名称不能为空")]
        [StringLength(30, ErrorMessage = "标签名称长度不能长于30")]
        public string? Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(500, ErrorMessage = "标签描述长度不能超过500")]
        public string? Description { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Required]
        public StatusEnum Status { get; set; } = StatusEnum.Normal;

        /// <summary>
        /// 自定义验证逻辑
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Name?.Length < 2)
            {
                yield return new ValidationResult("标签名称长度不能小于2", new[] { nameof(Name) });
            }
        }
    }
}
