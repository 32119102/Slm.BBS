using ContIn.Abp.Terminal.Application.Contracts.Messages;
using ContIn.Abp.Terminal.Application.Contracts.Users;
using ContIn.Abp.Terminal.Domain.Shared;
using ContIn.Abp.Terminal.Domain.Shared.Enum;
using ContIn.Abp.Terminal.Domain.Shared.Options;
using ContIn.Abp.Terminal.Domain.Users;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Timing;

namespace ContIn.Abp.Terminal.Application.CustomMappers
{
    /// <summary>
    /// 消息对象映射
    /// </summary>
    public class MessageObjectMapper : IObjectMapper<List<Message>, List<MessageDto>>, ITransientDependency
    {
        private readonly AppOptions _appOptions;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IUserRepository _userRepository;
        private readonly IClock _clock;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"></param>
        /// <param name="jsonSerializer"></param>
        /// <param name="userRepository"></param>
        /// <param name="clock"></param>
        public MessageObjectMapper(IOptionsMonitor<AppOptions> options, IJsonSerializer jsonSerializer, IUserRepository userRepository, IClock clock)
        {
            _appOptions = options.CurrentValue;
            _jsonSerializer = jsonSerializer;
            _userRepository = userRepository;
            _clock = clock;
        }

        /// <summary>
        /// 对象映射
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public List<MessageDto> Map(List<Message> source)
        {
            var destination = new List<MessageDto>();
            return Map(source, destination);
        }

        /// <summary>
        /// 对象映射
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public List<MessageDto> Map(List<Message> source, List<MessageDto> destination)
        {
            source.ForEach(s =>
            {
                var dest = new MessageDto();
                var from = BuildUserInfoDefaultIfNull(s.FromId).Result;
                if (s.FromId <= 0)
                {
                    from.Nickname = "系统通知";
                }
                dest.MessageId = s.Id;
                dest.FromId = s.FromId;
                dest.From = from;
                dest.DetailUrl = GetMessageDetailUrl(s);
                dest.UserId = s.UserId;
                dest.Title = s.Title;
                dest.Content = s.Content;
                dest.QuoteContent = s.QuoteContent;
                dest.Type = s.Type;
                dest.ExtraData = s.ExtraData;
                dest.Status = s.Status;
                dest.CreateTime = s.CreateTime;
                destination.Add(dest);
            });
            return destination;
        }

        /// <summary>
        /// 生成消息详情链接
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private string GetMessageDetailUrl(Message message)
        {
            var msgType = message.Type;
            if (msgType == MessageType.TopicComment || msgType == MessageType.ArticleComment || msgType == MessageType.CommentReply)
            {
                var extraData = _jsonSerializer.Deserialize<MessageEntityExtraDataDto>(message.ExtraData);
                if (extraData.EntityType!.Equals(EntityTypeEnum.Article.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return _appOptions.SiteBaseUrl + "/article/" + extraData.EntityId;
                }
                else if (extraData.EntityType!.Equals(EntityTypeEnum.Topic.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return _appOptions.SiteBaseUrl + "/topic/" + extraData.EntityId;
                }
            }
            else if (msgType == MessageType.TopicLike || msgType == MessageType.TopicFavorite || msgType == MessageType.TopicRecommend)
            {
                var extraData = _jsonSerializer.Deserialize<MessageTopicExtraDataDto>(message.ExtraData);
                if (extraData?.TopicId > 0)
                {
                    return _appOptions.SiteBaseUrl + "/topic/" + extraData.TopicId;
                }
            }
            return _appOptions.SiteBaseUrl + "/user/messages";
        }
        /// <summary>
        /// 构建用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<UserDto> BuildUserInfoDefaultIfNull(long userId)
        { 
            var user = await _userRepository.FindAsync(userId);
            if (user == null)
            {
                user = new User()
                {
                    UserName = userId.ToString(),
                    Nickname = "匿名用户" + userId.ToString(),
                    CreateTime = _clock.Now.ToMillisecondsTimestamp()
                };
            }
            var userDto = new UserDto()
            {
                Id = user.Id,
                Nickname = user.Nickname,
                UserName = user.UserName,
                Avatar = user.Avatar,
                TopicCount = user.TopicCount,
                CommentCount = user.CommentCount,
                FansCount = user.FansCount,
                FollowCount = user.FollowCount,
                Score = user.Score,
                Description = user.Description,
                CreateTime = user.CreateTime,
            };
            if (userDto.Description.IsNullOrWhiteSpace())
            {
                userDto.Description = "这家伙很懒，什么都没留下";
            }
            if (user.Status == StatusEnum.Delete)
            {
                userDto.Nickname = "黑名单用户";
                userDto.Description = "";
            }
            return userDto;
        }

    }
}
