using System.ComponentModel;
using Volo.Abp.Application.Dtos;

namespace ContIn.Abp.Terminal.Application.Contracts.Comments
{
    /// <summary>
    /// 评论搜索
    /// </summary>
    public class SearchCommentPagedDto : PagedResultRequestDto
    {
        /// <summary>
        /// 评论编号
        /// </summary>
        [DefaultValue("")]
        public string? Id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [DefaultValue("")]
        public string? UserName { get; set; }
        /// <summary>
        ///评论对象
        /// </summary>
        [DefaultValue("")]
        public string? EntityType { get; set; }
        /// <summary>
        /// 对象编号
        /// </summary>
        [DefaultValue("")]
        public string? EntityId { get; set; }
        /// <summary>
        /// 状态 0-正常 1-删除
        /// </summary>
        [DefaultValue("0")]
        public string? Status { get; set; }
    }
}
