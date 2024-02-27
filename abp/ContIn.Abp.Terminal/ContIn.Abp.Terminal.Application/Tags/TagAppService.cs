using ContIn.Abp.Terminal.Application.Contracts;
using ContIn.Abp.Terminal.Application.Contracts.Tags;
using ContIn.Abp.Terminal.Domain.Shared;
using ContIn.Abp.Terminal.Domain.Tags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;

namespace ContIn.Abp.Terminal.Application.Tags
{
    /// <summary>
    /// 标签应用服务
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = SwaggerGroupName.ArticleTopic)]
    public class TagAppService : ApplicationService, ITagAppService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IDistributedCache<TagDto, long> _cache;
        private readonly TagManager _tagManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="cache"></param>
        /// <param name="tagManager"></param>
        public TagAppService(ITagRepository repository, IDistributedCache<TagDto, long> cache, TagManager tagManager)
        {
            _tagRepository = repository;
            _cache = cache;
            _tagManager = tagManager;
        }

        /// <summary>
        /// 获取标签信息
        /// </summary>
        /// <param name="id">标签编号</param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<TagDto> GetAsync(long id)
        {
            return await _cache.GetOrAddAsync(id,
                async () => 
                {
                    var t = await _tagRepository.FindAsync(id);
                    if (t == null)
                    {
                        // throw new CustomException(TerminalErrorCodes.EntityNotFound, "未查询到标签信息");
#pragma warning disable CS8603 // 可能返回 null 引用。
                        return null;
#pragma warning restore CS8603 // 可能返回 null 引用。
                    }
                    return ObjectMapper.Map<Tag, TagDto>(t);
                });
        }
        /// <summary>
        /// 获取列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<TagDto>> GetListAsync(GetTagListDto input)
        {
            var tags = await _tagRepository.GetListAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Name
            );

            var totalCount = input.Name == null
                ? await _tagRepository.GetCountAsync()
                : await _tagRepository.GetCountAsync(input.Name);

            return new PagedResultDto<TagDto>(
                totalCount,
                ObjectMapper.Map<List<Tag>, List<TagDto>>(tags)
            );
        }
        /// <summary>
        /// 新增标签信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<TagDto> CreateAsync(CreateUpdateTagDto input)
        {
            var tag = await _tagManager.CreateAsync(input.Name!, input.Description, input.Status);

            var resultTag = await _tagRepository.InsertAsync(tag);
            return ObjectMapper.Map<Tag, TagDto>(resultTag);
        }
        /// <summary>
        /// 修改标签信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateAsync(long id, CreateUpdateTagDto input)
        {
            var tag = await _tagRepository.FindAsync(id);
            if (tag == null)
            {
                throw new CustomException(TerminalErrorCodes.EntityNotFound, "没有找到此标签编号的信息");
            }

            if (tag.Name != input.Name)
            {
                await _tagManager.ChangeNameAsync(tag, input.Name!);
            }

            tag.Description = input.Description;
            tag.Status = input.Status;
            tag.UpdateTime = Clock.Now.ToMillisecondsTimestamp();

            await _tagRepository.UpdateAsync(tag);
            await _cache.RemoveAsync(id);
        }
        /// <summary>
        /// 删除标签信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(long id)
        {
            await _tagRepository.DeleteAsync(id);
            await _cache.RemoveAsync(id);
        }

        /// <summary>
        /// 匹配标签
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<List<TagSimpleDto>> GetAutoCompleteAsync(string? name)
        {
            var tags = await _tagRepository.GetListAsync(0, 10, name);
            return ObjectMapper.Map<List<Tag>, List<TagSimpleDto>>(tags);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<SitePagingResultDto<TagSimpleDto>> GetTagsAsync(int page)
        {
            int limit = 100;
            page = page < 1 ? 1 : page;
            var result = await _tagRepository.GetPagingAsync(page, limit);
            return new SitePagingResultDto<TagSimpleDto>()
            {
                Page = new SitePagingPage()
                {
                    Limit = limit,
                    Page = page,
                    Total = result.Item1
                },
                Results = ObjectMapper.Map<List<Tag>, List<TagSimpleDto>>(result.Item2)
            };
        }
    }
}
