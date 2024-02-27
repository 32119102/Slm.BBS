using Volo.Abp.Application.Dtos;

namespace ContIn.Abp.Terminal.Application.Contracts.Links
{
    /// <summary>
    /// 列表查询
    /// </summary>
    public class GetFriendlyLinkListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 链接地址
        /// </summary>
        public string? Url { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }
    }
}
