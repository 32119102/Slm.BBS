using Volo.Abp.Application.Dtos;

namespace ContIn.Abp.Terminal.Application.Contracts.Topics
{
    /// <summary>
    /// 节点
    /// </summary>
    public class GetTopicNodeListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
    }
}
