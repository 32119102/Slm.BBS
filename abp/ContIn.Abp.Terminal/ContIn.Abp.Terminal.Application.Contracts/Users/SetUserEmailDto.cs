using System.ComponentModel.DataAnnotations;

namespace ContIn.Abp.Terminal.Application.Contracts.Users
{
    /// <summary>
    /// 设置邮箱
    /// </summary>
    public class SetUserEmailDto
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        [Required]
        public string? Email { get; set; }
    }
}
