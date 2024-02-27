using System.ComponentModel.DataAnnotations;

namespace ContIn.Abp.Terminal.Application.Contracts.Links
{
    /// <summary>
    /// 采集网站名称和描述
    /// </summary>
    public class GetDetectInputDto
    {
        /// <summary>
        /// 链接地址
        /// </summary>
        [Required(ErrorMessage = "链接地址不能为空")]
        public string? Url { get; set; }
    }
}
