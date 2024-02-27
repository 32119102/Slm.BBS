using ContIn.Abp.Terminal.Application.Contracts.Topics;
using Volo.Abp.Application.Services;

namespace ContIn.Abp.Terminal.Application.Contracts.Search
{
    /// <summary>
    /// 搜索
    /// </summary>
    public interface ISearchAppService : IApplicationService
    {
        /// <summary>
        /// 搜索话题列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<SitePagingResultDto<TopicSimpleDto>> PostTopicsAsync(SearchInputDto input);
    }
}
