using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ContIn.Abp.Terminal.Application.Contracts.Tags
{
    /// <summary>
    /// 标签
    /// </summary>
    public interface ITagAppService : IApplicationService
    {
        /// <summary>
        /// 获取标签信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TagDto> GetAsync(long id);
        /// <summary>
        /// 列表查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<TagDto>> GetListAsync(GetTagListDto input);
        /// <summary>
        /// 新增标签信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<TagDto> CreateAsync(CreateUpdateTagDto input);
        /// <summary>
        /// 修改标签信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateAsync(long id, CreateUpdateTagDto input);
        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(long id);
        /// <summary>
        /// 标签模糊匹配
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<List<TagSimpleDto>> GetAutoCompleteAsync(string? name);
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<SitePagingResultDto<TagSimpleDto>> GetTagsAsync(int page);
    }
}
