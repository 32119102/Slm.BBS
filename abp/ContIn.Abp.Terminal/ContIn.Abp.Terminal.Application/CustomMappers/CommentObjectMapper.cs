using ContIn.Abp.Terminal.Application.Contracts.Comments;
using ContIn.Abp.Terminal.Application.Contracts.Topics;
using ContIn.Abp.Terminal.Application.Contracts.Users;
using ContIn.Abp.Terminal.Domain.Comments;
using ContIn.Abp.Terminal.Domain.Shared.Enum;
using ContIn.Abp.Terminal.Domain.Users;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.ObjectMapping;

namespace ContIn.Abp.Terminal.Application.CustomMappers
{
    /// <summary>
    /// 评论列表对象映射
    /// </summary>
    public class CommentListObjectMapper : IObjectMapper<List<Comment>, List<CommentSimpleDto>>, ITransientDependency
    {
        private readonly IObjectMapper _mapper;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapper"></param>
        public CommentListObjectMapper(IObjectMapper mapper)
        { 
            _mapper = mapper;
        }
        /// <summary>
        /// 对象映射
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public List<CommentSimpleDto> Map(List<Comment> source)
        {
            var destination = new List<CommentSimpleDto>();
            return Map(source, destination);
        }
        /// <summary>
        /// 对象映射
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public List<CommentSimpleDto> Map(List<Comment> source, List<CommentSimpleDto> destination)
        {
            source.ForEach(x =>
            {
                destination.Add(_mapper.Map<Comment, CommentSimpleDto>(x));
            });
            return destination;
        }
    }
    /// <summary>
    /// 评论对象映射
    /// </summary>
    public class CommentObjectMapper : IObjectMapper<Comment, CommentSimpleDto>, ITransientDependency
    {
        private readonly IUserLikeAppService _userLikeAppService;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IObjectMapper _mapper;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userLikeAppService"></param>
        /// <param name="mapper"></param>
        /// <param name="jsonSerializer"></param>
        public CommentObjectMapper(IUserLikeAppService userLikeAppService, IObjectMapper mapper, IJsonSerializer jsonSerializer)
        { 
            _userLikeAppService = userLikeAppService;
            _mapper = mapper;
            _jsonSerializer = jsonSerializer;
        }
        /// <summary>
        /// 对象映射
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public CommentSimpleDto Map(Comment source)
        {
            var dest = new CommentSimpleDto();
            return Map(source, dest);
        }
        /// <summary>
        /// 对象映射
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public CommentSimpleDto Map(Comment source, CommentSimpleDto destination)
        {
            destination.CommentId = source.Id;
            destination.User = source.User != null ? _mapper.Map<User, UserSimpleDto>(source.User) : null;
            destination.EntityType = source.EntityType;
            destination.EntityId = source.EntityId;
            destination.Content = source.Content;
            destination.ImageList = _jsonSerializer.Deserialize<List<TopicImageDto>>(source.ImageList ?? "[]");
            destination.ContentType = source.ContentType;
            destination.QuoteId = source.QuoteId;
            destination.LikeCount = source.LikeCount;
            destination.CommentCount = source.CommentCount;
            destination.CreateTime = source.CreateTime;
            destination.Liked = _userLikeAppService.GetLikedAsync(EntityTypeEnum.Comment.ToString().ToLower(), source.Id).Result;

            return destination;
        }
    }
}
