using System.ComponentModel.DataAnnotations;

namespace ContIn.Abp.Terminal.Application.Contracts.Authorize
{
    /// <summary>
    /// 后台登录
    /// </summary>
    public class UserPwdLoginDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "用户名/邮箱不能为空")]
        public string? UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        public string? Password { get; set; }
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
    }
}
