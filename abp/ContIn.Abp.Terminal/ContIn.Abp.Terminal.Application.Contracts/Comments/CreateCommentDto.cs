using ContIn.Abp.Terminal.Domain.Shared.Enum;
using System.ComponentModel.DataAnnotations;

namespace ContIn.Abp.Terminal.Application.Contracts.Comments
{
    /// <summary>
    /// 评论
    /// </summary>
    public class CreateCommentDto
    {
        /// <summary>
        /// 评论内容类型
        /// 默认text
        /// </summary>
        public string? ContentType { get; set; } = ContentTypeEnum.Text.ToString().ToLower();

        /// <summary>
        /// 实体类型
        /// </summary>
        [Required(ErrorMessage = "实体类型不能为空")]
        public string? EntityType { get; set; }

        /// <summary>
        /// 实体编号
        /// </summary>
        public long EntityId { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        [Required(ErrorMessage = "评论内容不能为空"), StringLength(2000, ErrorMessage = "评论内容过长")]
        public string? Content { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string? ImageList { get; set; }

        /// <summary>
        /// 引用评论编号
        /// </summary>
        public string? QuoteId { get; set; }
    }
}
