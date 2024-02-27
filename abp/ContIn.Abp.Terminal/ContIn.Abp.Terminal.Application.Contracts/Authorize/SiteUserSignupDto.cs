using System.ComponentModel.DataAnnotations;

namespace ContIn.Abp.Terminal.Application.Contracts.Authorize
{
    /// <summary>
    /// 注册
    /// </summary>
    public class SiteUserSignupDto : IValidatableObject
    { 
        /// <summary>
        /// 昵称
        /// </summary>
        [Required(ErrorMessage = "昵称不能为空")]
        public string? Nickname { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "用户名不能为空")]
        public string? Username { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        [Required(ErrorMessage = "邮箱不能为空")]
        public string? Email { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        public string? Password { get; set; }
        /// <summary>
        /// 确认密码
        /// </summary>
        [Required(ErrorMessage = "确认密码不能为空")]
        public string? RePassword { get; set; }

        /// <summary>
        /// 验证码编号
        /// </summary>
        [Required]
        public string? CaptchaId { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = "验证码不能为空")]
        public string? CaptchaCode { get; set; }

        /// <summary>
        /// 自定义验证逻辑
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Password!.Equals(RePassword, StringComparison.OrdinalIgnoreCase))
            {
                yield return new ValidationResult("两次输入密码不一致", new[] { nameof(Password), nameof(RePassword) });
            }
        }
    }
}
