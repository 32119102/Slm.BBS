using ContIn.Abp.Terminal.Domain.Shared.Enum;
using System.ComponentModel.DataAnnotations;

namespace ContIn.Abp.Terminal.Application.Contracts.Users
{
    /// <summary>
    /// 新增编辑用户对象
    /// </summary>
    public class CreateUpdateUserDto : IValidatableObject
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "用户名不能为空")]
        public string? UserName { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [Required(ErrorMessage = "昵称不能为空")]
        public string? Nickname { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        [Required(ErrorMessage = "邮箱不能为空")]
        public string? Email { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public string? Roles { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string? Password { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public StatusEnum Status { get; set; } = StatusEnum.Normal;
        /// <summary>
        /// 自定义验证逻辑
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Password.IsNullOrWhiteSpace() && Password!.Length < 6)
            {
                yield return new ValidationResult("密码长度不能小于6", new[] { nameof(Password) });
            }
        }
    }
}
