using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ContIn.Abp.Terminal.Application.Contracts.Links
{
    /// <summary>
    /// 友情链接
    /// </summary>
    public interface IFriendlyLinkAppService : IApplicationService
    {
        /// <summary>
        /// 获取单条信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<FriendlyLinkDto> GetAsync(long id);
        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<FriendlyLinkDto>> PostListAsync(GetFriendlyLinkListDto input);
        /// <summary>
        /// 新增信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<FriendlyLinkDto> CreateAsync(CreateUpdateFriendlyLinkDto input);
        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateAsync(long id, CreateUpdateFriendlyLinkDto input);
        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(long id);
        /// <summary>
        /// 采集网站信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetDetectOutputDto> PostDetectAsync(GetDetectInputDto input);
        /// <summary>
        /// 获取前10个链接
        /// </summary>
        /// <returns></returns>
        Task<List<FriendlyLinkSimpleDto>> GetTopLinksAsync();
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<SitePagingResultDto<FriendlyLinkSimpleDto>> GetLinksAsync(int page);
    }
}
