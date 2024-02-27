using System.ComponentModel.DataAnnotations;

namespace ContIn.Abp.Terminal.Application.Contracts.Users
{
    /// <summary>
    /// 个人资料修改
    /// </summary>
    public class UpdateUserInfoDto
    {
        /// <summary>
        /// 昵称
        /// </summary>
        [Required(ErrorMessage = "昵称不能为空")]
        public string? NickName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// 个人主页
        /// </summary>
        public string? HomePage { get; set; }
    }
}
