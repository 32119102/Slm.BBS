using ContIn.Abp.Terminal.Application.Contracts;
using ContIn.Abp.Terminal.Application.Contracts.Comments;
using ContIn.Abp.Terminal.Application.Contracts.Sys;
using ContIn.Abp.Terminal.Application.Contracts.Topics;
using ContIn.Abp.Terminal.Application.Contracts.Users;
using ContIn.Abp.Terminal.Application.LocalEventBus;
using ContIn.Abp.Terminal.Core.Helpers;
using ContIn.Abp.Terminal.Domain.Comments;
using ContIn.Abp.Terminal.Domain.Shared;
using ContIn.Abp.Terminal.Domain.Shared.Enum;
using ContIn.Abp.Terminal.Domain.Topics;
using ContIn.Abp.Terminal.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Auditing;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Json;

namespace ContIn.Abp.Terminal.Application.Comments
{
    /// <summary>
    /// 评论
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = SwaggerGroupName.ArticleTopic)]
    public class CommentAppService : ApplicationService, ICommentAppService
    {
        private readonly ICommentRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IUserAppService _userAppService;
        private readonly UserManager _userManager;
        private readonly IAuditingHelper _auditingHelper;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly ITopicRepository _topicRepository;
        private readonly IUserLikeAppService _userLikeAppService;
        private readonly ISysConfigAppService _sysConfigService;
        private readonly ILocalEventBus _localEventBus;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="userRepository"></param>
        /// <param name="userManager"></param>
        /// <param name="auditingHelper"></param>
        /// <param name="jsonSerializer"></param>
        /// <param name="topicRepository"></param>
        /// <param name="userLikeAppService"></param>
        /// <param name="sysConfigAppService"></param>
        /// <param name="userAppService"></param>
        /// <param name="localEventBus"></param>
        public CommentAppService(ICommentRepository repository, IUserRepository userRepository, UserManager userManager, IAuditingHelper auditingHelper, IJsonSerializer jsonSerializer,
            ITopicRepository topicRepository, IUserLikeAppService userLikeAppService, ISysConfigAppService sysConfigAppService
            , IUserAppService userAppService
            , ILocalEventBus localEventBus)
        { 
            _repository = repository;
            _userRepository = userRepository;
            _userManager = userManager;
            _auditingHelper = auditingHelper;
            _jsonSerializer = jsonSerializer;
            _topicRepository = topicRepository;
            _userLikeAppService = userLikeAppService;
            _sysConfigService = sysConfigAppService;
            _userAppService = userAppService;
            _localEventBus = localEventBus;
        }
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<CommentDto>> PostPagedAsync(SearchCommentPagedDto input)
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
            var comments = await _repository.GetListAsync(input.SkipCount, input.MaxResultCount, input.Id.IsNullOrWhiteSpace() ? 0 : Convert.ToInt64(input.Id)
                , userIds.ToArray(), input.EntityType, input.EntityId.IsNullOrWhiteSpace() ? 0 : Convert.ToInt64(input.EntityId) 
                , input.Status.IsNullOrWhiteSpace() ? null : (StatusEnum)Enum.Parse(typeof(StatusEnum), input.Status!), true);

            var count = await _repository.GetCountAsync(input.Id.IsNullOrWhiteSpace() ? 0 : Convert.ToInt64(input.Id), userIds.ToArray(), 
                input.EntityType, input.EntityId.IsNullOrWhiteSpace() ? 0 : Convert.ToInt64(input.EntityId), 
                input.Status.IsNullOrWhiteSpace() ? null : (StatusEnum)Enum.Parse(typeof(StatusEnum), input.Status!));

            return new PagedResultDto<CommentDto>(count, ObjectMapper.Map<List<Comment>, List<CommentDto>>(comments));
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(long id)
        {
            await _repository.DeleteAsync(id);
        }

        /// <summary>
        /// 获取评论
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="entityId"></param>
        /// <param name="cursor"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<SitePagedResultDto<CommentSimpleDto>> GetCommentsAsync(string entityType, long entityId, long cursor = 0)
        {
            int limit = 10;
            var comments = await _repository.GetCommentsAsync(entityType, entityId, cursor, limit, true);
            return new SitePagedResultDto<CommentSimpleDto>()
            {
                Cursor = comments.Count > 0 ? comments.Last().Id : cursor,
                HasMore = comments.Count >= limit,
                Results = await BuildCommentsAsync(comments, true, true)
            };
        }

        /// <summary>
        /// 构建评论列表
        /// </summary>
        /// <param name="comments"></param>
        /// <param name="isBuildReplies">是否渲染评论的二级回复，一级评论时要设置为true，其他时候都为false</param>
        /// <param name="isBuildQuote">是否渲染评论的引用，二级回复时要设置为true，其他时候都为false</param>
        /// <returns></returns>
        private async Task<List<CommentSimpleDto>> BuildCommentsAsync(List<Comment> comments, bool isBuildReplies, bool isBuildQuote)
        {
            List<CommentSimpleDto> rets = new List<CommentSimpleDto>();
            foreach (var comment in comments)
            {
                var commentDto = ObjectMapper.Map<Comment, CommentSimpleDto>(comment);
                // 评论内容格式化

                // 是否渲染二级评论
                if (isBuildReplies && comment.CommentCount > 0)
                {
                    var repliesLimit = 3;
                    var replies = await _repository.GetRepliesAsync(comment.Id, 0, repliesLimit, true);
                    var repliesDtos = await BuildCommentsAsync(replies, false, true);
                    commentDto.Replies = new SitePagedResultDto<CommentSimpleDto>()
                    {
                        Results = repliesDtos,
                        HasMore = replies.Count >= repliesLimit,
                        Cursor = replies.Count > 0 ? replies.Last().Id : 0
                    };
                }

                // 是否渲染引用评论
                if (isBuildQuote && comment.QuoteId > 0)
                {
                    var quoteComment = await _repository.GetAsync(comment.QuoteId, true);
                    var quote = await BuildCommentsAsync(new List<Comment>() { quoteComment }, false, false);
                    commentDto.Quote = quote.FirstOrDefault();
                }
                rets.Add(commentDto);
            }
            return rets;
        }

        /// <summary>
        /// 新增评论
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<CommentSimpleDto> PostCreateAsync(CreateCommentDto input)
        {
            var userId = CurrentUser.GetUserId();
            var user = await _userRepository.GetAsync(userId);
            // 系统配置
            var config = await _sysConfigService.GetAllAsync();
            // 用户状态检查
            await _userManager.CheckUserStatusAsync(user.Status, user.ForbiddenEndTime, true);
            var auditInfo = _auditingHelper.CreateAuditLogInfo();
            var comment = new Comment()
            {
                UserId = userId,
                EntityType = input.EntityType,
                EntityId = input.EntityId,
                Content = input.Content,
                ContentType = input.ContentType,
                QuoteId = input.QuoteId.IsNullOrWhiteSpace() ? 0 : Convert.ToInt64(input.QuoteId),
                Status = StatusEnum.Normal,
                UserAgent = auditInfo.BrowserInfo,
                IP = auditInfo.ClientIpAddress,
                CreateTime = Clock.Now.ToMillisecondsTimestamp(),
                ImageList = input.ImageList.IsNullOrWhiteSpace() ? "" : _jsonSerializer.Serialize(_jsonSerializer.Deserialize<List<TopicImageDto>>(input.ImageList))
            };
            // 保存评论
            var resultComment = await _repository.InsertAsync(comment, true);
            if (comment.EntityType!.Equals(EntityTypeEnum.Topic.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                // 当帖子被评论的时候，更新最后回复时间、回复数量+1
                await _topicRepository.OnCommentAsync(input.EntityId, userId, comment.CreateTime);
            }
            else if (comment.EntityType!.Equals(EntityTypeEnum.Comment.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                // 评论被回复（二级评论）
                await _repository.OnCommentAsync(comment.EntityId);
            }
            // 用户跟帖计数
            await _userRepository.IncrCommentCountAsync(userId);
            // 用户获得积分
            await _localEventBus.PublishAsync(new UserScoreChangedEvent()
            {
                UserId = userId,
                Score = config.ScoreConfig!.PostCommentScore,
                SourceId = resultComment.Id,
                SourceType = EntityTypeEnum.Comment,
                Description = "发表跟帖",
                Type = ScoreTypeEnum.Incre
            });
            // 发送消息
            await _localEventBus.PublishAsync(new CommentCreateEvent()
            {
                UserId = userId,
                CommentId = resultComment.Id,
                EntityType = comment.EntityType,
                EntityId = comment.EntityId,
                QuoteId = comment.QuoteId,
                SummaryCommentContent = MarkdigHelper.GetSummary(comment.Content, Constants.CommentSummaryLength)
            });

            var resultComments = await BuildCommentsAsync(new List<Comment>() { resultComment }, true, true);
            return resultComments.First();
        }

        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public async Task PostLikedAsync(long commentId)
        {
            var userId = CurrentUser.GetUserId();
            var existing = await _userLikeAppService.GetLikedAsync(EntityTypeEnum.Comment.ToString().ToLower(), commentId);
            if (!existing)
            {
                await _userLikeAppService.InsertAsync(new CreateUserLikeDto()
                {
                    UserId = userId,
                    EntityType = EntityTypeEnum.Comment.ToString().ToLower(),
                    EntityId = commentId
                });
                // 更新话题点赞数量
                await _repository.IncrLikeCountAsync(commentId);
            }
        }

        /// <summary>
        /// 获取评论回复
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="cursor"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<SitePagedResultDto<CommentSimpleDto>> GetRepliesAsync(long commentId, long cursor = 0)
        {
            var limit = 3;
            var replies = await _repository.GetRepliesAsync(commentId, cursor, limit, true);
            return new SitePagedResultDto<CommentSimpleDto>()
            {
                Cursor = replies.Count > 0 ? replies.Last().Id : cursor,
                HasMore = replies.Count >= limit,
                Results = await BuildCommentsAsync(replies, false, true)
            };
        }
    }
}
