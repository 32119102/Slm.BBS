using System.ComponentModel.DataAnnotations;

namespace ContIn.Abp.Terminal.Application.Contracts.Users
{
    /// <summary>
    /// 设置用户名
    /// </summary>
    public class SetUsernameDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string? Username { get; set; }
    }
}
