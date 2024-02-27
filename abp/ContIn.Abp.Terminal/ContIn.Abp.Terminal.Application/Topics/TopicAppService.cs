using ContIn.Abp.Terminal.Application.Contracts.Captcha;
using ContIn.Abp.Terminal.Application.Contracts.Feed;
using ContIn.Abp.Terminal.Application.Contracts.Sys;
using ContIn.Abp.Terminal.Application.Contracts.Topics;
using ContIn.Abp.Terminal.Application.Contracts.Users;
using ContIn.Abp.Terminal.Application.LocalEventBus;
using ContIn.Abp.Terminal.Domain.Shared;
using ContIn.Abp.Terminal.Domain.Shared.Enum;
using ContIn.Abp.Terminal.Domain.Tags;
using ContIn.Abp.Terminal.Domain.Topics;
using ContIn.Abp.Terminal.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Auditing;
using Volo.Abp.Caching;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Json;

namespace ContIn.Abp.Terminal.Application.Topics
{
    /// <summary>
    /// 话题
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = SwaggerGroupName.ArticleTopic)]
    public class TopicAppService : ApplicationService, ITopicAppService
    {
        private readonly ITopicRepository _repository;
        private readonly IUserAppService _userService;
        private readonly IUserRepository _userRepository;
        private readonly ITopicNodeRepository _nodeRepository;
        private readonly IUserLikeAppService _userLikeAppService;
        private readonly IUserFavoriteAppService _userFavoriteAppService;
        private readonly ISysConfigAppService _sysConfigService;
        private readonly ICaptchaAppService _captchaService;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IAuditingHelper _auditingHelper;
        private readonly ITagRepository _tagRepository;
        private readonly ITopicTagRepository _topicTagRepository;
        private readonly IUserFeedAppService _userFeedAppService;
        private readonly ILocalEventBus _localEventBus;
        private readonly IDistributedCache<TopicSimpleDto, long> _simpleCache;
        private readonly IDistributedCache<TopicDetailDto, long> _detailCache;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="userService">用户服务</param>
        /// <param name="userRepository"></param>
        /// <param name="nodeRepository"></param>
        /// <param name="userLikeAppService">用户点赞服务</param>
        /// <param name="userFavoriteAppService">用户收藏服务</param>
        /// <param name="sysConfigAppService">系统配置服务</param>
        /// <param name="captchaAppService">验证码服务</param>
        /// <param name="jsonSerializer"></param>
        /// <param name="auditingHelper"></param>
        /// <param name="tagRepository"></param>
        /// <param name="topicTagRepository"></param>
        /// <param name="userFeedAppService"></param>
        /// <param name="localEventBus"></param>
        /// <param name="detailCache"></param>
        /// <param name="simpleCache"></param>
        public TopicAppService(ITopicRepository repository, IUserAppService userService, IUserRepository userRepository, ITopicNodeRepository nodeRepository, IUserLikeAppService userLikeAppService
            , IUserFavoriteAppService userFavoriteAppService, ISysConfigAppService sysConfigAppService, ICaptchaAppService captchaAppService
            , IJsonSerializer jsonSerializer, IAuditingHelper auditingHelper, ITagRepository tagRepository, ITopicTagRepository topicTagRepository
            , IUserFeedAppService userFeedAppService
            , ILocalEventBus localEventBus
            , IDistributedCache<TopicSimpleDto, long> simpleCache
            , IDistributedCache<TopicDetailDto, long> detailCache)
        {
            _repository = repository;
            _userService = userService;
            _userRepository = userRepository;
            _nodeRepository = nodeRepository;
            _userLikeAppService = userLikeAppService;
            _userFavoriteAppService = userFavoriteAppService;
            _sysConfigService = sysConfigAppService;
            _captchaService = captchaAppService;
            _jsonSerializer = jsonSerializer;
            _auditingHelper = auditingHelper;
            _tagRepository = tagRepository;
            _topicTagRepository = topicTagRepository;
            _userFeedAppService = userFeedAppService;
            _localEventBus = localEventBus;
            _simpleCache = simpleCache;
            _detailCache = detailCache;
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task RemoveCacheAsync(long id)
        { 
            await _simpleCache.RemoveAsync(id);
            await _detailCache.RemoveAsync(id);
        }

        /// <summary>
        /// 发表话题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task<TopicDetailDto> PostCreateAsync(CreateUpdateTopicDto input)
        {
            var userId = CurrentUser.GetUserId();
            // 用户状态检查
            var user = await _userService.GetAsync(userId);
            await _userService.CheckUserStatusAsync(user!);
            // 命中发布评论的策略

            // 系统配置
            var config = await _sysConfigService.GetAllAsync();
            // 节点取默认节点，如果有的话
            if (input.NodeId <= 0)
            {
                input.NodeId = config.DefaultNodeId;
            }
            if (input.NodeId <= 0)
            {
                throw new CustomException(TerminalErrorCodes.CommonErrorCode, "请选择节点");
            }
            // 判断节点是否存在
            var node = await _nodeRepository.FindAsync(input.NodeId, false);
            if (node == null || node.Status != StatusEnum.Normal)
            {
                throw new CustomException(TerminalErrorCodes.EntityNotFound, "节点不存在");
            }
            // 判断是否需要验证验证码
            if (config.TopicCaptcha)
            {
                await _captchaService.ValideCaptchaCodeAsync(input.CaptchaId, input.CaptchaCode);
            }
            var auditInfo = _auditingHelper.CreateAuditLogInfo();
            var topic = new Topic()
            {
                Type = input.Type,
                NodeId = input.NodeId,
                UserId = userId,
                Title = input.Title,
                Content = input.Content,
                ImageList = input.ImageList.IsNullOrWhiteSpace() ? "": _jsonSerializer.Serialize(_jsonSerializer.Deserialize<List<TopicImageDto>>(input.ImageList)),
                Status = StatusEnum.Normal,
                LastCommentTime = Clock.Now.ToMillisecondsTimestamp(),
                UserAgent = auditInfo.BrowserInfo,
                IP = auditInfo.ClientIpAddress,
                CreateTime = Clock.Now.ToMillisecondsTimestamp(),
            };
            // 保存标签
            var tags = await _tagRepository.GetOrCreateAsync((input.Tags ?? "").Split(","), Clock.Now.ToMillisecondsTimestamp());
            // 保存话题
            topic = await _repository.InsertAsync(topic, true);
            // 保存话题标签关系
            await _topicTagRepository.AddTopicTags(topic.Id, tags.Select(x => x.Id).ToArray(), Clock.Now.ToMillisecondsTimestamp());
            // 用户话题计数
            await _userService.IncrTopicCountAsync(userId);
            // 获得积分
            //if ((config.ScoreConfig?.PostTopicScore ?? 0) > 0)
            //{
            //    await _userService.AddScoreAsync(userId, config.ScoreConfig!.PostTopicScore, EntityTypeEnum.Topic, topic.Id, "发表话题");
            //}
            await _localEventBus.PublishAsync(new UserScoreChangedEvent()
            {
                UserId = userId,
                SourceType = EntityTypeEnum.Topic,
                SourceId = topic.Id,
                Description = "发表话题",
                Type = ScoreTypeEnum.Incre,
                Score = config.ScoreConfig!.PostTopicScore
            });
            // 新增Feed流
            await _userFeedAppService.CreateByUserAsync(userId, topic.Id, EntityTypeEnum.Topic);

            return ObjectMapper.Map<Topic, TopicDetailDto>(topic);
        }

        /// <summary>
        /// 修改话题
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task<TopicDetailDto> PutUpdateAsync(long id, CreateUpdateTopicDto input)
        {
            var userId = CurrentUser.GetUserId();
            // 用户状态检查
            var user = await _userService.GetAsync(userId);
            await _userService.CheckUserStatusAsync(user!);

            var topic = await _repository.GetAsync(id, true);
            if (topic.Status != StatusEnum.Normal)
            {
                throw new CustomException(TerminalErrorCodes.EntityNotFound, "话题不存在");
            }
            // 非作者，且非管理员
            if (topic.UserId != userId 
                && !await _userService.HasAnyRoleAsync(user?.Roles, RoleTypeEnum.Owner) 
                && !await _userService.HasAnyRoleAsync(user?.Roles, RoleTypeEnum.Admin))
            {
                throw new CustomException(TerminalErrorCodes.AuthorizeUserNotExistsOrForbidden, "无权限");
            }
            // 系统配置
            var config = await _sysConfigService.GetAllAsync();
            // 节点取默认节点，如果有的话
            if (input.NodeId <= 0)
            {
                input.NodeId = config.DefaultNodeId;
            }
            if (input.NodeId <= 0)
            {
                throw new CustomException(TerminalErrorCodes.CommonErrorCode, "请选择节点");
            }
            // 判断节点是否存在
            var node = await _nodeRepository.FindAsync(input.NodeId, false);
            if (node == null || node.Status != StatusEnum.Normal)
            {
                throw new CustomException(TerminalErrorCodes.EntityNotFound, "节点不存在");
            }
            var auditInfo = _auditingHelper.CreateAuditLogInfo();
            topic.NodeId = input.NodeId;
            topic.Title = input.Title;
            topic.Content = input.Content;
            topic.ImageList = input.ImageList.IsNullOrWhiteSpace() ? "" : _jsonSerializer.Serialize(_jsonSerializer.Deserialize<List<TopicImageDto>>(input.ImageList));

            // 保存标签
            var tags = await _tagRepository.GetOrCreateAsync((input.Tags ?? "").Split(","), Clock.Now.ToMillisecondsTimestamp());
            // 删除现有话题标签关系
            topic!.TopicTags?.ToList().ForEach(x =>
            {
                _topicTagRepository.DeleteAsync(x);
            });
            // 保存话题
            topic = await _repository.UpdateAsync(topic);
            // 保存话题标签关系
            await _topicTagRepository.AddTopicTags(topic.Id, tags.Select(x => x.Id).ToArray(), Clock.Now.ToMillisecondsTimestamp());

            await RemoveCacheAsync(id);

            return ObjectMapper.Map<Topic, TopicDetailDto>(topic);
        }

        /// <summary>
        /// 获取话题详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<TopicDetailDto> GetAsync(long id)
        {
            var topic = await _detailCache.GetOrAddAsync(id,
                async () =>
                {
                    var topic = await _repository.GetAsync(id, true);
                    if (topic == null || topic.Status != StatusEnum.Normal)
                    {
                        throw new CustomException(TerminalErrorCodes.EntityNotFound, "话题不存在");
                    }
                    
                    return ObjectMapper.Map<Topic, TopicDetailDto>(topic);
                });
            await _repository.IncrViewCount(id);
            await RemoveCacheAsync(id);
            return topic;
        }

        /// <summary>
        /// 获取话题信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        [RemoteService(false)]
        public async Task<TopicSimpleDto?> GetSimpleAsync(long id)
        {
            return await _simpleCache.GetOrAddAsync(id,
                async () =>
                {
                    var topic = await _repository.GetAsync(id, true);
                    if (topic == null)
                    {
#pragma warning disable CS8603 // 可能返回 null 引用。
                        return null;
#pragma warning restore CS8603 // 可能返回 null 引用。
                    }
                    return ObjectMapper.Map<Topic, TopicSimpleDto>(topic);
                });
        }

        /// <summary>
        /// 收藏
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task PostFavoriteAsync(long topicId)
        {
            var userId = CurrentUser.GetUserId();
            var topic = await _repository.FindAsync(topicId, false);
            if (topic == null || topic.Status != StatusEnum.Normal)
            {
                throw new CustomException(TerminalErrorCodes.EntityNotFound, "话题不存在");
            }
            await _userFavoriteAppService.InsertAsync(userId, EntityTypeEnum.Topic.ToString().ToLower(), topicId);
            // 发布本地事件
            await _localEventBus.PublishAsync(new TopicOperateEvent()
            {
                UserId = userId,
                EntityId = topicId,
                EntityType = EntityTypeEnum.Topic.ToString(),
                MessageType = MessageType.TopicFavorite
            });
        }

        /// <summary>
        /// 获取节点信息
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<TopicNodeSimpleDto> GetNodeAsync(long nodeId)
        {
            var node = await _nodeRepository.FindAsync(nodeId);
            return ObjectMapper.Map<TopicNode, TopicNodeSimpleDto>(node);
        }

        /// <summary>
        /// 获取所有节点信息
        /// 未登录状态也可以查询到
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<List<TopicNodeSimpleDto>> GetNodesAsync()
        {
            var nodes = await _nodeRepository.GetListAsync(0, 20, string.Empty);
            return ObjectMapper.Map<List<TopicNode>, List<TopicNodeSimpleDto>>(nodes);
        }

        /// <summary>
        /// 获取最近点赞信息
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<List<UserSimpleDto>> GetRecentLikedAsync(long topicId)
        {
            var likes = await _userLikeAppService.GetRecentAsync(EntityTypeEnum.Topic.ToString().ToLower(), topicId);
            var likeUsers = likes?.Where(x => x.User != null).Select(x => x.User).ToList();
            return ObjectMapper.Map<List<UserDto>, List<UserSimpleDto>>(likeUsers!);
        }

        /// <summary>
        /// 获取话题列表
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="recommend"></param>
        /// <param name="cursor"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<TopicPagedResultDto> GetTopicsAsync(long nodeId = 0, bool recommend = false, long cursor = 0)
        {
            var limit = 10;
            var topics = await _repository.GetPagedTopicsAsync(nodeId, recommend, cursor, limit, false);
            // var topicDtos = ObjectMapper.Map<List<Topic>, List<TopicSimpleDto>>(topics);
            List<TopicSimpleDto> topicDtos = await IncludeTopicDetailsAsync(topics);
            return new TopicPagedResultDto()
            {
                Cursor = topics?.Count > 0 ? topics.Last().LastCommentTime : cursor,
                HasMore = topics?.Count >= limit,
                Results = topicDtos
            };
        }

        /// <summary>
        /// 填充话题详情
        /// </summary>
        /// <param name="topics"></param>
        /// <returns></returns>
        private async Task<List<TopicSimpleDto>> IncludeTopicDetailsAsync(List<Topic>? topics)
        {
            var userId = CurrentUser.GetUserId();
            List<TopicSimpleDto> topicDtos = new List<TopicSimpleDto>();
            if (topics != null)
            {
                foreach (var t in topics)
                {
                    var topic = await GetSimpleAsync(t.Id);
                    if (topic == null)
                        continue;

                    // 用户信息
                    topic.User = await _userService.GetSimpleUserAsync(t.UserId);
                    // 是否点赞
                    topic.Liked = userId == 0 ? false : await _userLikeAppService.GetLikedAsync(EntityTypeEnum.Topic.ToString().ToLower(), t.Id);

                    topicDtos.Add(topic);
                }
            }
            return topicDtos;
        }

        /// <summary>
        /// 获取置顶话题
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<List<TopicSimpleDto>> GetStickyTopicsAsync(long nodeId = 0)
        {
            int limit = 3;
            var topics = await _repository.GetStickyTopicsAsync(nodeId, limit, false);
            List<TopicSimpleDto> topicDtos = await IncludeTopicDetailsAsync(topics);
            //return ObjectMapper.Map<List<Topic>, List<TopicSimpleDto>>(topics);
            return topicDtos;
        }

        /// <summary>
        /// 设置话题置顶
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="sticky"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task PostStickyAsync(long topicId, bool sticky = false)
        {
            var userId = CurrentUser.GetUserId();
            var user = await _userService.GetAsync(userId);
            // 判断角色权限
            if (!await _userService.HasAnyRoleAsync(user?.Roles, RoleTypeEnum.Owner) 
                && !await _userService.HasAnyRoleAsync(user?.Roles, RoleTypeEnum.Admin))
            {
                throw new CustomException(TerminalErrorCodes.AuthorizeUserNotExistsOrForbidden, "无权限");
            }
            await _repository.SetStickyAsync(topicId, sticky, Clock.Now.ToMillisecondsTimestamp());
        }

        /// <summary>
        /// 获取用户话题列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cursor">游标，上一个话题的编号</param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<TopicPagedResultDto> GetUserTopicsAsync(long userId, long cursor)
        {
            var limit = 10;
            var topics = await _repository.GetUserTopicsAsync(userId, cursor, limit, false);
            // var topicDtos = ObjectMapper.Map<List<Topic>, List<TopicSimpleDto>>(topics);
            List<TopicSimpleDto> topicDtos = await IncludeTopicDetailsAsync(topics);
            return new TopicPagedResultDto()
            {
                Cursor = topics?.Count > 0 ? topics.Last().Id : cursor,
                HasMore = topics?.Count >= limit,
                Results = topicDtos
            };
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<TopicDto>> PostPagedAsync(SearchTopicPagedDto input)
        {
            List<long> userIds = new List<long>();
            if (!input.UserName.IsNullOrWhiteSpace())
            { 
                var users = await _userRepository.FindUsersLikeUserNameAsync(input.UserName!);
                if (users != null)
                {
                    userIds.AddRange(users.Select(x => x.Id).ToList());
                }
            }
            var topics = await _repository.GetListAsync(input.SkipCount, input.MaxResultCount, userIds.ToArray(), input.Title
                , input.Recommend.IsNullOrWhiteSpace() ? null : input.Recommend == "1"
                , input.Sticky.IsNullOrWhiteSpace() ? null : input.Sticky == "1"
                , input.Status.IsNullOrWhiteSpace() ? null : (StatusEnum)Enum.Parse(typeof(StatusEnum), input.Status!), true);

            var count = await _repository.GetCountAsync(userIds.ToArray(), input.Title
                , input.Recommend.IsNullOrWhiteSpace() ? null : input.Recommend == "1"
                , input.Sticky.IsNullOrWhiteSpace() ? null : input.Sticky == "1"
                , input.Status.IsNullOrWhiteSpace() ? null : (StatusEnum)Enum.Parse(typeof(StatusEnum), input.Status!));

            return new PagedResultDto<TopicDto>(count, ObjectMapper.Map<List<Topic>, List<TopicDto>>(topics));
        }

        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task SetDeleteAsync(long id, string status)
        {
            var t = await _repository.GetAsync(id, false);
            t.Status = status.ToEnum<StatusEnum>();
            await _repository.UpdateAsync(t);
            await RemoveCacheAsync(id);
            if (t.Status == StatusEnum.Delete)
            {
                // 发布本地事件
                await _localEventBus.PublishAsync(new TopicOperateEvent()
                {
                    UserId = CurrentUser.GetUserId(),
                    EntityId = id,
                    EntityType = EntityTypeEnum.Topic.ToString(),
                    MessageType = MessageType.TopicDelete
                });
            }
        }

        /// <summary>
        /// 设置推荐
        /// </summary>
        /// <param name="id"></param>
        /// <param name="recommend"></param>
        /// <returns></returns>
        public async Task SetRecommendAsync(long id, bool recommend)
        {
            var userId = CurrentUser.GetUserId();
            var user = await _userService.GetAsync(userId);
            var t = await _repository.GetAsync(id, false);

            // 非管理员
            if (!await _userService.HasAnyRoleAsync(user?.Roles, RoleTypeEnum.Owner) 
                && !await _userService.HasAnyRoleAsync(user?.Roles, RoleTypeEnum.Admin))
            {
                throw new CustomException(TerminalErrorCodes.AuthorizeUserNotExistsOrForbidden, "无权限");
            }

            t.Recommend = recommend;
            t.RecommendTime = Clock.Now.ToMillisecondsTimestamp();
            await _repository.UpdateAsync(t);
            await RemoveCacheAsync(id);
            // 发布本地事件
            if (recommend)
            {
                await _localEventBus.PublishAsync(new TopicOperateEvent()
                {
                    UserId = userId,
                    EntityId = id,
                    EntityType = EntityTypeEnum.Topic.ToString(),
                    MessageType = MessageType.TopicRecommend
                });
            }
        }

        /// <summary>
        /// 删除话题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task PostDeleteAsync(long id)
        {
            var userId = CurrentUser.GetUserId();
            // 检查用户状态
            var user = await _userService.GetCurrentAsync(true);
            await _userService.CheckUserStatusAsync(user!);

            var topic = await _repository.GetAsync(id, false);
            if (topic.Status != StatusEnum.Normal)
            {
                return;
            }
            // 非作者，且非管理员
            if (topic.UserId != userId
                && !await _userService.HasAnyRoleAsync(user?.Roles, RoleTypeEnum.Owner)
                && !await _userService.HasAnyRoleAsync(user?.Roles, RoleTypeEnum.Admin))
            {
                throw new CustomException(TerminalErrorCodes.AuthorizeUserNotExistsOrForbidden, "无权限");
            }
            await SetDeleteAsync(id, "1");
            // 设置话题标签删除状态
            await _topicTagRepository.SetDeleteAsync(id);
            // 取消feed流
            await _userFeedAppService.DeleteByDataIdAsync(id, EntityTypeEnum.Topic);
            // 移除缓存
            await RemoveCacheAsync(id);
            // 发布本地事件
            await _localEventBus.PublishAsync(new TopicOperateEvent()
            {
                UserId = userId,
                EntityId = id,
                EntityType = EntityTypeEnum.Topic.ToString(),
                MessageType = MessageType.TopicDelete
            });
        }

        /// <summary>
        /// 获取标签相关文章
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="cursor"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<TopicPagedResultDto> GetTagTopicsAsync(long tagId, long cursor = 0)
        {
            int limit = 10;
            var topicTags = await _topicTagRepository.GetTagTopicsAsync(tagId, cursor, limit);
            var topics = new List<TopicSimpleDto>();
            topicTags?.ForEach(x =>
            {
                var topic = GetSimpleAsync(x.TopicId).Result;
                if (topic != null && topic.Status == StatusEnum.Normal)
                {
                    topics.Add(topic);
                }
            });
            return new TopicPagedResultDto()
            {
                Cursor = topicTags?.Count > 0 ? topicTags.Last().Id : cursor,
                HasMore = topicTags?.Count >= limit,
                Results = topics
            };
        }

        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public async Task PostLikeAsync(long topicId)
        {
            var userId = CurrentUser.GetUserId();
            var existing = await _userLikeAppService.GetLikedAsync(EntityTypeEnum.Topic.ToString().ToLower(), topicId);
            if (!existing)
            {
                await _userLikeAppService.InsertAsync(new CreateUserLikeDto()
                {
                    UserId = userId,
                    EntityType = EntityTypeEnum.Topic.ToString().ToLower(),
                    EntityId = topicId
                });
                // 更新话题点赞数量
                await _repository.IncrLikeCountAsync(topicId);
                await RemoveCacheAsync(topicId);
                // 发布本地事件
                await _localEventBus.PublishAsync(new TopicOperateEvent()
                {
                    EntityType = EntityTypeEnum.Topic.ToString(),
                    EntityId = topicId,
                    UserId = userId,
                    MessageType = MessageType.TopicLike
                });
            }
        }

        /// <summary>
        /// 获取关注人的话题列表
        /// </summary>
        /// <param name="cursor"></param>
        /// <returns></returns>
        public async Task<TopicPagedResultDto> GetFeedsAsync(long cursor = 0)
        {
            int limit = 10;
            var userId = CurrentUser.GetUserId();
            var feeds = await _userFeedAppService.GetUserFeedsAsync(userId, cursor, EntityTypeEnum.Topic, limit);

            List<TopicSimpleDto> topics = new List<TopicSimpleDto>();
            if (feeds != null)
            {
                foreach (var f in feeds)
                {
                    var topic = await GetSimpleAsync(f.DataId);
                    if (topic == null || topic.Status != StatusEnum.Normal)
                        continue;
                    topics.Add(topic);
                }
            }
            return new TopicPagedResultDto()
            {
                Cursor = feeds?.Count > 0 ? feeds.Last().CreateTime : cursor,
                HasMore = feeds?.Count >= limit,
                Results = topics
            };
        }

    }
}
