using System.ComponentModel.DataAnnotations;

namespace ContIn.Abp.Terminal.Application.Contracts.Users
{
    /// <summary>
    /// 修改密码
    /// </summary>
    public class UpdateUserPasswordDto : IValidatableObject
    {
        /// <summary>
        /// 旧密码
        /// </summary>
        [Required]
        public string? OldPassword { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string? Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Required]
        public string? RePassword { get; set; }

        /// <summary>
        /// 自定义验证规则
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Password!.Equals(RePassword, StringComparison.OrdinalIgnoreCase))
            {
                yield return new ValidationResult("两次输入密码不匹配", new[] { nameof(Password), nameof(RePassword) });
            }
        }
    }
}
