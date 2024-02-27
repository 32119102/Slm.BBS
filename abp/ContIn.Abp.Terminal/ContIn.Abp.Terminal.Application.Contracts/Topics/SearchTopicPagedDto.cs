using System.ComponentModel;
using Volo.Abp.Application.Dtos;

namespace ContIn.Abp.Terminal.Application.Contracts.Topics
{
    /// <summary>
    /// 话题
    /// </summary>
    public class SearchTopicPagedDto : PagedResultRequestDto
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
        /// 是否推荐，0-否 1-是
        /// </summary>
        [DefaultValue("")]
        public string? Recommend { get; set; }
        /// <summary>
        /// 状态 0-正常 1-删除
        /// </summary>
        [DefaultValue("0")]
        public string? Status { get; set; }

        /// <summary>
        /// 是否置顶
        /// </summary>
        public string? Sticky { get; set; }
    }
}
