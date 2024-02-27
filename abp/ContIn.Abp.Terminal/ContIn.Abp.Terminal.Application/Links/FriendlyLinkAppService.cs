using ContIn.Abp.Terminal.Application.Contracts;
using ContIn.Abp.Terminal.Application.Contracts.Links;
using ContIn.Abp.Terminal.Core.Helpers;
using ContIn.Abp.Terminal.Domain.Links;
using ContIn.Abp.Terminal.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;

namespace ContIn.Abp.Terminal.Application.Links
{
    /// <summary>
    /// 友情链接
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = SwaggerGroupName.Common)]
    public class FriendlyLinkAppService : ApplicationService, IFriendlyLinkAppService
    {
        private readonly IFriendlyLinkRepository _repository;
        private readonly IDistributedCache<FriendlyLinkDto, long> _cache;
        private readonly IHttpClientFactory _httpClientFactory;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="cache"></param>
        /// <param name="httpClientFactory"></param>
        public FriendlyLinkAppService(IFriendlyLinkRepository repository, IDistributedCache<FriendlyLinkDto, long> cache, IHttpClientFactory httpClientFactory)
        { 
            _repository = repository;
            _cache = cache;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<FriendlyLinkDto> CreateAsync(CreateUpdateFriendlyLinkDto input)
        {
            var link = new FriendlyLink()
            {
                Url = input.Url,
                Title = input.Title,
                Summary = input.Summary,
                Logo =  input.Logo,
                Status = input.Status,
                CreateTime = Clock.Now.ToMillisecondsTimestamp()
            };

            var resultTag = await _repository.InsertAsync(link);
            return ObjectMapper.Map<FriendlyLink, FriendlyLinkDto>(resultTag);
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(long id)
        {
            await _repository.DeleteAsync(id);
            await _cache.RemoveAsync(id);
        }

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FriendlyLinkDto> GetAsync(long id)
        {
            return await _cache.GetOrAddAsync(id,
                async () =>
                {
                    var t = await _repository.FindAsync(id);
                    if (t == null)
                    {
                        throw new CustomException(TerminalErrorCodes.EntityNotFound, "未查询到友情链接信息");
                    }
                    return ObjectMapper.Map<FriendlyLink, FriendlyLinkDto>(t);
                });
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<SitePagingResultDto<FriendlyLinkSimpleDto>> GetLinksAsync(int page)
        {
            int limit = 10;
            page = page < 1 ? 1 : page;
            var result = await _repository.GetPagingAsync(page, limit);
            return new SitePagingResultDto<FriendlyLinkSimpleDto>()
            {
                Page = new SitePagingPage()
                { 
                    Limit = limit,
                    Page = page,
                    Total = result.Item1
                },
                Results = ObjectMapper.Map<List<FriendlyLink>, List<FriendlyLinkSimpleDto>>(result.Item2)
            };
        }

        /// <summary>
        /// 获取前10个链接
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<List<FriendlyLinkSimpleDto>> GetTopLinksAsync()
        {
            var links = await _repository.GetTopLinksAsync();
            return ObjectMapper.Map<List<FriendlyLink>, List<FriendlyLinkSimpleDto>>(links);
        }

        /// <summary>
        /// 采集网站信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetDetectOutputDto> PostDetectAsync(GetDetectInputDto input)
        {
            using var client = _httpClientFactory.CreateClient();
            var response = await client.GetStringAsync(input.Url);
            if (response.IsNullOrWhiteSpace())
            {
                throw new CustomException(TerminalErrorCodes.ArgumentIsEmpty, "读取网站内容为空");
            }
            var htmlDocument = new HtmlDocumentHelper(response);
            return new GetDetectOutputDto()
            {
                Title = htmlDocument.GetTitle(),
                Description = htmlDocument.GetDescription()
            };
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<FriendlyLinkDto>> PostListAsync(GetFriendlyLinkListDto input)
        {
            var links = await _repository.GetListAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Url,
                input.Title
            );

            var totalCount = await _repository.GetCountAsync(input.Url, input.Title);

            return new PagedResultDto<FriendlyLinkDto>(
                totalCount,
                ObjectMapper.Map<List<FriendlyLink>, List<FriendlyLinkDto>>(links)
            );
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task UpdateAsync(long id, CreateUpdateFriendlyLinkDto input)
        {
            var link = await _repository.FindAsync(id);
            if (link == null)
            {
                throw new CustomException(TerminalErrorCodes.EntityNotFound, "没有找到此链接编号的信息");
            }

            link.Url = input.Url;
            link.Title = input.Title;
            link.Summary = input.Summary;
            link.Logo = input.Logo;
            link.Status = input.Status;

            await _repository.UpdateAsync(link);
            await _cache.RemoveAsync(id);
        }
    }
}
