using ContIn.Abp.Terminal.Application.Contracts;
using ContIn.Abp.Terminal.Application.Contracts.Search;
using ContIn.Abp.Terminal.Application.Contracts.Topics;
using ContIn.Abp.Terminal.Domain.Shared;
using ContIn.Abp.Terminal.Domain.Topics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Services;

namespace ContIn.Abp.Terminal.Application.Search
{
    /// <summary>
    /// 搜索
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = SwaggerGroupName.ArticleTopic)]
    public class SearchAppService : ApplicationService, ISearchAppService
    {
        private readonly ITopicRepository _topicRepository;
        private readonly ITopicAppService _topicAppService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="topicRepository"></param>
        /// <param name="topicAppService"></param>
        public SearchAppService(ITopicRepository topicRepository, ITopicAppService topicAppService)
        { 
            _topicRepository = topicRepository;
            _topicAppService = topicAppService;
        }
        /// <summary>
        /// 搜索话题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        [AllowAnonymous]
        public async Task<SitePagingResultDto<TopicSimpleDto>> PostTopicsAsync(SearchInputDto input)
        {
            if (input.Keyword.IsNullOrWhiteSpace())
            {
                throw new CustomException(TerminalErrorCodes.CommonErrorCode, "请输入搜索关键词");
            }
            var recommend = input.NodeId == -1 ? true : false;
            long createTime = 0;
            switch (input.TimeRange)
            {
                case 0:
                    break;
                case 1:
                    createTime = Clock.Now.AddDays(-1).ToMillisecondsTimestamp();
                    break;
                case 2:
                    createTime = Clock.Now.AddDays(-7).ToMillisecondsTimestamp();
                    break;
                case 3:
                    createTime = Clock.Now.AddMonths(-1).ToMillisecondsTimestamp();
                    break;
                case 4:
                    createTime = Clock.Now.AddYears(-1).ToMillisecondsTimestamp();
                    break;
            }
            var limit = 10;
            var result = await _topicRepository.SearchTopicsAsync(input.Keyword!, input.NodeId, recommend, createTime, input.Page, limit, false);

            List<TopicSimpleDto> topicDtos = new List<TopicSimpleDto>();
            if (result.Item2 != null)
            {
                foreach (var t in result.Item2)
                {
                    var topic = await _topicAppService.GetSimpleAsync(t.Id);
                    if (topic == null)
                        continue;
                    topicDtos.Add(topic);
                }
            }

            return new SitePagingResultDto<TopicSimpleDto>()
            {
                Page = new SitePagingPage()
                {
                    Limit = limit,
                    Page = input.Page,
                    Total = result.Item1
                },
                Results = topicDtos
            };
        }
    }
}
