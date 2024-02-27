using ContIn.Abp.Terminal.Application.Contracts.Topics;
using ContIn.Abp.Terminal.Domain.Shared;
using ContIn.Abp.Terminal.Domain.Topics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;

namespace ContIn.Abp.Terminal.Application.Topics
{
    /// <summary>
    /// 节点应用服务
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = SwaggerGroupName.ArticleTopic)]
    public class TopicNodeAppService : ApplicationService, ITopicNodeAppService
    {
        private readonly ITopicNodeRepository _repository;
        private readonly IDistributedCache<TopicNodeDto, long> _cache;
        private readonly TopicNodeManager _manager;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="cache"></param>
        /// <param name="manager"></param>
        public TopicNodeAppService(ITopicNodeRepository repository, IDistributedCache<TopicNodeDto, long> cache, TopicNodeManager manager)
        { 
            _repository = repository;
            _cache = cache;
            _manager = manager;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<TopicNodeDto> CreateAsync(CreateUpdateTopicNodeDto input)
        {
            var tag = await _manager.CreateAsync(input.Name!, input.Description, input.SortNo, input.Status, input.Logo);
            var resultTag = await _repository.InsertAsync(tag);
            return ObjectMapper.Map<TopicNode, TopicNodeDto>(resultTag);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(long id)
        {
            await _repository.DeleteAsync(id);
            await _cache.RemoveAsync(id);
        }
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        [AllowAnonymous]
        public async Task<TopicNodeDto> GetAsync(long id)
        {
            return await _cache.GetOrAddAsync(id,
                async () =>
                {
                    var t = await _repository.FindAsync(id);
                    if (t == null)
                    {
                        // throw new CustomException(TerminalErrorCodes.EntityNotFound, "未查询到节点信息");
#pragma warning disable CS8603 // 可能返回 null 引用。
                        return null;
#pragma warning restore CS8603 // 可能返回 null 引用。
                    }
                    return ObjectMapper.Map<TopicNode, TopicNodeDto>(t);
                });
        }
        /// <summary>
        /// 列表查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<TopicNodeDto>> PostListAsync(GetTopicNodeListDto input)
        {
            var nodes = await _repository.GetListAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Name
            );
            var totalCount = await _repository.GetCountAsync(input.Name);
            return new PagedResultDto<TopicNodeDto>(
                totalCount,
                ObjectMapper.Map<List<TopicNode>, List<TopicNodeDto>>(nodes)
            );
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task UpdateAsync(long id, CreateUpdateTopicNodeDto input)
        {
            var node = await _repository.FindAsync(id);
            if (node == null)
            {
                throw new CustomException(TerminalErrorCodes.EntityNotFound, "没有找到此节点编号的信息");
            }

            if (node.Name != input.Name)
            {
                await _manager.ChangeNameAsync(node, input.Name!);
            }

            node.Description = input.Description;
            node.Status = input.Status;
            node.SortNo = input.SortNo;
            node.Logo = input.Logo;

            await _repository.UpdateAsync(node);
            await _cache.RemoveAsync(id);
        }
    }
}
