using System.ComponentModel;
using Volo.Abp.Application.Dtos;

namespace ContIn.Abp.Terminal.Application.Contracts.Articles
{
    /// <summary>
    /// 文章搜索
    /// </summary>
    public class SearchArticlePagedDto : PagedResultRequestDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [DefaultValue("")]
        public string? UserName { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [DefaultValue("")]
        public string? Title { get; set; }
        /// <summary>
        /// 状态 0-正常 1-删除 2- 待审核
        /// </summary>
        [DefaultValue("0")]
        public string? Status { get; set; }
    }
}
