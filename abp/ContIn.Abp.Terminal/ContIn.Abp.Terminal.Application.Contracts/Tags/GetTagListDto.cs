using Volo.Abp.Application.Dtos;

namespace ContIn.Abp.Terminal.Application.Contracts.Tags
{
    /// <summary>
    /// 列表查询
    /// </summary>
    public class GetTagListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 标签名称
        /// </summary>
        public string? Name { get; set; }
    }
}
