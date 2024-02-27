using ContIn.Abp.Terminal.Application.Contracts.Tags;
using ContIn.Abp.Terminal.Application.Contracts.Topics;
using ContIn.Abp.Terminal.Application.Contracts.Users;
using ContIn.Abp.Terminal.Core.Helpers;
using ContIn.Abp.Terminal.Domain.Shared;
using ContIn.Abp.Terminal.Domain.Shared.Enum;
using ContIn.Abp.Terminal.Domain.Tags;
using ContIn.Abp.Terminal.Domain.Topics;
using ContIn.Abp.Terminal.Domain.Users;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.ObjectMapping;

namespace ContIn.Abp.Terminal.Application.CustomMappers
{
    /// <summary>
    /// 话题数组对象映射
    /// </summary>
    public class TopicsObjectMapper : IObjectMapper<List<Topic>, List<TopicSimpleDto>>, ITransientDependency
    {
        private readonly IObjectMapper _mapper;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapper"></param>
        public TopicsObjectMapper(IObjectMapper mapper)
        { 
            _mapper = mapper;
        }
        /// <summary>
        /// 映射
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public List<TopicSimpleDto> Map(List<Topic> source)
        {
            var destination = new List<TopicSimpleDto>();
            return Map(source, destination);
        }
        /// <summary>
        /// 映射
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public List<TopicSimpleDto> Map(List<Topic> source, List<TopicSimpleDto> destination)
        {
            source.ForEach(s =>
            {
                destination.Add(_mapper.Map<Topic, TopicSimpleDto>(s));
            });
            return destination;
        }
    }
    /// <summary>
    /// 话题对象映射
    /// </summary>
    public class TopicObjectMapper : IObjectMapper<Topic, TopicSimpleDto>, ITransientDependency
    {
        private readonly IUserLikeAppService _userLikeAppService;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IObjectMapper _mapper;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userLikeAppService"></param>
        /// <param name="jsonSerializer"></param>
        /// <param name="mapper"></param>
        public TopicObjectMapper(IUserLikeAppService userLikeAppService, IJsonSerializer jsonSerializer
            , IObjectMapper mapper)
        { 
            _userLikeAppService = userLikeAppService;
            _jsonSerializer = jsonSerializer;
            _mapper = mapper;
        }
        /// <summary>
        /// 对象映射
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public TopicSimpleDto Map(Topic source)
        {
            var destination = new TopicSimpleDto();
            return Map(source, destination);
        }
        /// <summary>
        /// 对象映射
        /// </summary>
        /// <param name="s"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public TopicSimpleDto Map(Topic s, TopicSimpleDto destination)
        {
            destination = new TopicSimpleDto()
            {
                TopicId = s.Id,
                Type = s.Type,
                Title = s.Title,
                Summary = MarkdigHelper.GetSummary(s.Content, Constants.TopicSummaryLength),
                ImageList = _jsonSerializer.Deserialize<List<TopicImageDto>>(s.ImageList ?? "[]"),
                Recommend = s.Recommend,
                RecommendTime = s.RecommendTime,
                ViewCount = s.ViewCount,
                CommentCount = s.CommentCount,
                LikeCount = s.LikeCount,
                LastCommentTime = s.LastCommentTime,
                Status = s.Status,
                CreateTime = s.CreateTime,
                NodeId = s.NodeId,
                Sticky = s.Sticky,
                StickyTime = s.StickyTime,
                Liked = _userLikeAppService.GetLikedAsync(EntityTypeEnum.Topic.ToString().ToLower(), s.Id).Result,
                Node = s.Node == null ? null : _mapper.Map<TopicNode, TopicNodeSimpleDto>(s.Node),
                Tags = s.TopicTags == null ? null : _mapper.Map<List<Tag>, List<TagSimpleDto>>(s.TopicTags.Where(x => x.Tag != null).Select(x => x.Tag!).ToList()),
                User = s.User == null ? null : _mapper.Map<User, UserSimpleDto>(s.User)
            };
            return destination;
        }
    }

    /// <summary>
    /// 话题详情
    /// </summary>
    public class TopicDetailObjectMapper : IObjectMapper<Topic, TopicDetailDto>, ITransientDependency
    {
        private readonly IUserLikeAppService _userLikeAppService;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IObjectMapper _mapper;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userLikeAppService"></param>
        /// <param name="jsonSerializer"></param>
        /// <param name="mapper"></param>
        public TopicDetailObjectMapper(IUserLikeAppService userLikeAppService, IJsonSerializer jsonSerializer, IObjectMapper mapper)
        {
            _userLikeAppService = userLikeAppService;
            _jsonSerializer = jsonSerializer;
            _mapper = mapper;
        }
        /// <summary>
        /// 对象映射
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public TopicDetailDto Map(Topic source)
        {
            var destination = new TopicDetailDto();
            return Map(source, destination);
        }
        /// <summary>
        /// 对象映射
        /// </summary>
        /// <param name="s"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public TopicDetailDto Map(Topic s, TopicDetailDto destination)
        {
            var dest = new TopicDetailDto()
            {
                TopicId = s.Id,
                Type = s.Type,
                Title = s.Title,
                Content = s.Content,
                Summary = MarkdigHelper.GetSummary(s.Content, Constants.TopicSummaryLength),
                ImageList = _jsonSerializer.Deserialize<List<TopicImageDto>>(s.ImageList ?? "[]"),
                Recommend = s.Recommend,
                RecommendTime = s.RecommendTime,
                ViewCount = s.ViewCount,
                CommentCount = s.CommentCount,
                LikeCount = s.LikeCount,
                LastCommentTime = s.LastCommentTime,
                Status = s.Status,
                CreateTime = s.CreateTime,
                NodeId = s.NodeId,
                Sticky = s.Sticky,
                StickyTime = s.StickyTime,
                Liked = _userLikeAppService.GetLikedAsync(EntityTypeEnum.Topic.ToString().ToLower(), s.Id).Result,
                Node = s.Node == null ? null : _mapper.Map<TopicNode, TopicNodeSimpleDto>(s.Node),
                Tags = s.TopicTags == null ? null : _mapper.Map<List<Tag>, List<TagSimpleDto>>(s.TopicTags.Where(x => x.Tag != null).Select(x => x.Tag!).ToList()),
                User = s.User == null ? null : _mapper.Map<User, UserSimpleDto>(s.User)
            };
            destination = dest;
            return destination;
        }
    }
}
