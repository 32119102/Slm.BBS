using System.ComponentModel.DataAnnotations;

namespace ContIn.Abp.Terminal.Application.Contracts.Articles
{
    /// <summary>
    /// 新增编辑文字
    /// </summary>
    public class CreateUpdateArticleDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "标题不能为空"), StringLength(50, ErrorMessage = "标题过长")]
        public string? Title { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string? Summary { get; set; }

        /// <summary>
        /// 文字内容
        /// </summary>
        [Required(ErrorMessage = "内容不能为空")]
        public string? Content { get; set; }

        /// <summary>
        /// 标签，逗号分隔
        /// </summary>
        public string? Tags { get; set; }
    }
}
