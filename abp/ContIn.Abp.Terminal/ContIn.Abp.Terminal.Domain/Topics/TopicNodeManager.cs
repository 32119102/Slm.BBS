using ContIn.Abp.Terminal.Domain.Shared;
using ContIn.Abp.Terminal.Domain.Shared.Enum;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Timing;

namespace ContIn.Abp.Terminal.Domain.Topics
{
    /// <summary>
    /// 节点管理
    /// </summary>
    public class TopicNodeManager : DomainService
    {
        private readonly ITopicNodeRepository _repository;
        private readonly IClock _clock;
        public TopicNodeManager(ITopicNodeRepository repository, IClock clock)
        {
            _repository = repository;
            _clock = clock;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="sortNo"></param>
        /// <param name="status"></param>
        /// <param name="logo"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task<TopicNode> CreateAsync(string name, string? description, int sortNo, StatusEnum status, string? logo)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existing = await _repository.FindByNameAsync(name);
            if (existing != null)
            {
                throw new CustomException(TerminalErrorCodes.EntityAlreadyExists, "节点名称已存在");
            }
            return new TopicNode
            {
                Name = name,
                Description = description,
                Status = status,
                SortNo = sortNo,
                Logo = logo,
                CreateTime = _clock.Now.ToMillisecondsTimestamp()
            };
        }
        /// <summary>
        /// 修改节点名称
        /// </summary>
        /// <param name="node"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task ChangeNameAsync(TopicNode node, string newName)
        {
            Check.NotNull(node, nameof(node));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _repository.FindByNameAsync(newName);
            if (existing != null && existing.Id != node.Id)
            {
                throw new CustomException(TerminalErrorCodes.EntityAlreadyExists, "节点名称已存在");
            }
            node.Name = newName;
        }
    }
}
