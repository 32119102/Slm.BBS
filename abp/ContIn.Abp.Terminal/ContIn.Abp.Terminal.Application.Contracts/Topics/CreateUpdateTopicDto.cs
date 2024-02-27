using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContIn.Abp.Terminal.Application.Contracts.Topics
{
    /// <summary>
    /// 话题
    /// </summary>
    public class CreateUpdateTopicDto : IValidatableObject
    {
        /// <summary>
        /// 类型 0-话题
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "标题不能为空")]
        [StringLength(128, ErrorMessage = "标题长度不能超过128")]
        public string? Title { get; set; }
        /// <summary>
        /// 节点
        /// 系统有默认几点
        /// </summary>
        public long NodeId { get; set; }
        /// <summary>
        /// 内容
        /// 图片和内容不能同时为空
        /// </summary>
        public string? Content { get; set; }
        /// <summary>
        /// 图片
        /// 图片和内容不能同时为空
        /// </summary>
        public string? ImageList { get; set; }

        /// <summary>
        /// 标签，逗号分隔
        /// </summary>
        public string? Tags { get; set; }

        /// <summary>
        /// 验证码编号
        /// </summary>
        public string? CaptchaId { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string? CaptchaCode { get; set; }

        /// <summary>
        /// 自定义验证规则
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Type != 0 && Type != 1)
            {
                yield return new ValidationResult("发表类型参数错误", new[] { nameof(Type) });
            }
            if (Content.IsNullOrWhiteSpace() && ImageList.IsNullOrWhiteSpace())
            {
                yield return new ValidationResult("内容和图片不能同时为空", new[] { nameof(Content), nameof(ImageList) });
            }
        }
    }
}
