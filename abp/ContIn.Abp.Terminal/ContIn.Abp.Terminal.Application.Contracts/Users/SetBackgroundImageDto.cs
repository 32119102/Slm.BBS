using System.ComponentModel.DataAnnotations;

namespace ContIn.Abp.Terminal.Application.Contracts.Users
{
    /// <summary>
    /// 设置背景图片
    /// </summary>
    public class SetBackgroundImageDto
    {
        /// <summary>
        /// 图片地址
        /// </summary>
        [Required(ErrorMessage = "请上传图片")]
        public string? Url { get; set; }
    }
}
