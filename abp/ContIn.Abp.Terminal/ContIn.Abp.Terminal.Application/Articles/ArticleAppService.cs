using ContIn.Abp.Terminal.Application.Contracts;
using ContIn.Abp.Terminal.Application.Contracts.Articles;
using ContIn.Abp.Terminal.Application.Contracts.Sys;
using ContIn.Abp.Terminal.Application.Contracts.Users;
using ContIn.Abp.Terminal.Core.Helpers;
using ContIn.Abp.Terminal.Domain.Articles;
using ContIn.Abp.Terminal.Domain.Shared;
using ContIn.Abp.Terminal.Domain.Shared.Enum;
using ContIn.Abp.Terminal.Domain.Tags;
using ContIn.Abp.Terminal.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Threading;

namespace ContIn.Abp.Terminal.Application.Articles
{
    /// <summary>
    /// 文章
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = SwaggerGroupName.ArticleTopic)]
    public class ArticleAppService : ApplicationService, IArticleAppService
    {
        private readonly IArticleRepository _repository;
        private readonly IArticleTagRepository _articleTagRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserAppService _userAppService;
        private readonly IUserFavoriteAppService _userFavoriteAppService;
        private readonly ISysConfigAppService _sysConfigAppService;
        private readonly ITagRepository _tagRepository;
        private readonly IDistributedCache<ArticleSimpleDto, long> _simpleCache;
        private readonly IDistributedCache<ArticleDetailDto, long> _detailCache;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="userRepository"></param>
        /// <param name="userAppService"></param>
        /// <param name="articleTagRepository"></param>
        /// <param name="userFavoriteAppService"></param>
        /// <param name="sysConfigAppService"></param>
        /// <param name="tagRepository"></param>
        /// <param name="simpleCache"></param>
        /// <param name="detailCache"></param>
        public ArticleAppService(IArticleRepository repository
            , IUserRepository userRepository
            , IUserAppService userAppService
            , IArticleTagRepository articleTagRepository,
            IUserFavoriteAppService userFavoriteAppService, ISysConfigAppService sysConfigAppService, ITagRepository tagRepository
            , IDistributedCache<ArticleDetailDto, long> detailCache
            , IDistributedCache<ArticleSimpleDto, long> simpleCache)
        {
            _repository = repository;
            _userRepository = userRepository;
            _userAppService = userAppService;
            _articleTagRepository = articleTagRepository;
            _userFavoriteAppService = userFavoriteAppService;
            _sysConfigAppService = sysConfigAppService;
            _tagRepository = tagRepository;
            _detailCache = detailCache;
            _simpleCache = simpleCache;
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
        /// 删除文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(long id)
        {
            await _repository.DeleteAsync(id);
            await RemoveCacheAsync(id);
        }

        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="cursor"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<SitePagedResultDto<ArticleSimpleDto>> GetArticlesAsync(long cursor, int limit = 10)
        {
            var articles = await _repository.GetArticlesAsync(cursor, limit, false);
            var articleDtos = await IncludeArticleDetailsAsync(articles);
            return new SitePagedResultDto<ArticleSimpleDto>()
            {
                Cursor = articles?.Count > 0 ? articles.Last().Id : cursor,
                HasMore = articles?.Count >= limit,
                Results = articleDtos
            };
        }

        #region 填充文章详情
        /// <summary>
        /// 填充文章详情
        /// </summary>
        /// <param name="articles"></param>
        /// <returns></returns>
        private async Task<List<ArticleSimpleDto>> IncludeArticleDetailsAsync(List<Article>? articles)
        { 
            List<ArticleSimpleDto> articleSimpleDtos = new List<ArticleSimpleDto>();
            if (articles != null)
            {
                foreach (var article in articles)
                {
                    var dto = await GetSimpleAsync(article.Id);
                    if (dto == null)
                        continue;

                    // 用户信息
                    // dto.User = await _userAppService.GetSimpleUserAsync(article.UserId);

                    articleSimpleDtos.Add(dto);
                }
            }
            return articleSimpleDtos;
        }
        #endregion

        /// <summary>
        /// 获取文章详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<ArticleDetailDto> GetAsync(long id)
        {
            var article = await _detailCache.GetOrAddAsync(id,
                async () =>
                {
                    var article = await _repository.GetAsync(id, true);
                    if (article == null || article.Status == StatusEnum.Delete)
                    {
                        throw new CustomException(TerminalErrorCodes.EntityNotFound, "文章不存在");
                    }
                    if (article.Status == StatusEnum.Auditting)
                    {
                        throw new CustomException(TerminalErrorCodes.EntityNotFound, "文章审核中");
                    }
                    return ObjectMapper.Map<Article, ArticleDetailDto>(article);
                });
            await _repository.IncrViewCount(id);
            await RemoveCacheAsync(id);
            return article;
        }

        /// <summary>
        /// 获取文章信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ArticleSimpleDto?> GetSimpleAsync(long id)
        {
            return await _simpleCache.GetOrAddAsync(id,
                async () =>
                {
                    var article = await _repository.GetAsync(id, true);
#pragma warning disable CS8603 // 可能返回 null 引用。
                    return article == null ? null : ObjectMapper.Map<Article, ArticleSimpleDto>(article);
#pragma warning restore CS8603 // 可能返回 null 引用。
                });
        }

        /// <summary>
        /// 收藏
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task PostFavoriteAsync(long articleId)
        {
            var userId = CurrentUser.GetUserId();
            var article = await GetSimpleAsync(articleId);
            if (article == null || article.Status != StatusEnum.Normal)
            {
                throw new CustomException(TerminalErrorCodes.EntityNotFound, "文章不存在");
            }
            await _userFavoriteAppService.InsertAsync(userId, EntityTypeEnum.Article.ToString().ToLower(), articleId);
        }

        /// <summary>
        /// 获取近期文章
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<List<ArticleSimpleDto>> GetNearlyAsync(long articleId)
        {
            var articles = await _repository.GetNearlyArticlesAsync(articleId, 10, false);
            var articleDtos = await IncludeArticleDetailsAsync(articles);
            return articleDtos;
            // return ObjectMapper.Map<List<Article>, List<ArticleSimpleDto>>(articles);
        }

        /// <summary>
        /// 获取相关文章
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<List<ArticleSimpleDto>> GetRelatedAsync(long articleId)
        {
            var tags = await _articleTagRepository.GetArticleTagsByArticleIdAsync(articleId);
            if (tags == null)
            {
                return new List<ArticleSimpleDto>();
            }
            var tagIds = tags.Select(t => t.TagId).ToArray();
            var articleTags = await _articleTagRepository.GetArticleTagsByTagIdsAsync(tagIds, 30, false);
            var articleIds = articleTags.Select(t => t.ArticleId).Take(10).ToList();
            var articles = new List<ArticleSimpleDto>();
            articleIds.ForEach(id =>
            {
                if (id != articleId && !articles.Where(x => x.ArticleId == id).Any())
                {
                    var article = AsyncHelper.RunSync(async () => await GetSimpleAsync(id));
                    if (article != null)
                    {
                        articles.Add(article);
                    }
                }
            });
            return articles;
        }

        /// <summary>
        /// 获取文章标签信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<ArticleTagDto>> GetTagsAsync(long id)
        {
            var tags = await _articleTagRepository.GetArticleTagsByArticleIdAsync(id, true);
            return ObjectMapper.Map<List<ArticleTag>, List<ArticleTagDto>>(tags ?? new List<ArticleTag>());
        }

        /// <summary>
        /// 获取用户文章列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cursor"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<SitePagedResultDto<ArticleSimpleDto>> GetUserArticlesAsync(long userId, long cursor = 0, int limit = 10)
        {
            var articles = await _repository.GetUserArticlesAsync(userId, cursor, limit, false);
            var articleDtos = await IncludeArticleDetailsAsync(articles);
            return new SitePagedResultDto<ArticleSimpleDto>()
            {
                Cursor = articles?.Count > 0 ? articles.Last().Id : cursor,
                HasMore = articles?.Count >= limit,
                Results = articleDtos
            };
        }

        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task PendingAsync(long id)
        {
            await _repository.PendingToNormalAsync(id);
            await RemoveCacheAsync(id);
        }

        /// <summary>
        /// 新增文章
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ArticleSimpleDto> PostCreateAsync(CreateUpdateArticleDto input)
        {
            var userId = CurrentUser.GetUserId();
            // 检查用户状态
            var user = await _userAppService.GetCurrentAsync(true);
            await _userAppService.CheckUserStatusAsync(user!);

            var article = new Article()
            {
                UserId = userId,
                Title = input.Title,
                Content = input.Content,
                ContentType = ContentTypeEnum.Markdown.ToString().ToLower(),
                // Summary = input.Summary,
                Summary = MarkdigHelper.GetSummary(input.Content, Constants.ArticleSummaryLength),
                Status = StatusEnum.Normal,
                CreateTime = Clock.Now.ToMillisecondsTimestamp(),
                UpdateTime = Clock.Now.ToMillisecondsTimestamp()
            };
            // 判断是否开启审核
            var sysConfigs = await _sysConfigAppService.GetAllAsync();
            if ((sysConfigs?.ArticlePending ?? false) == true)
            {
                article.Status = StatusEnum.Auditting;
            }
            // 保存标签
            var tags = await _tagRepository.GetOrCreateAsync((input.Tags ?? "").Split(","), Clock.Now.ToMillisecondsTimestamp());
            // 新增文章
            article = await _repository.InsertAsync(article, true);
            // 保存文章标签关系
            await _articleTagRepository.AddArticleTagsAsync(article.Id, tags.Select(x => x.Id).ToArray(), Clock.Now.ToMillisecondsTimestamp());

            return ObjectMapper.Map<Article, ArticleSimpleDto>(article);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id">文章编号</param>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task<ArticleSimpleDto> UpdateAsync(long id, CreateUpdateArticleDto input)
        {
            var userId = CurrentUser.GetUserId();
            // 检查用户状态
            var user = await _userAppService.GetCurrentAsync(true);
            await _userAppService.CheckUserStatusAsync(user!);

            var article = await _repository.GetAsync(id, true);
            if (article.Status == StatusEnum.Delete)
            {
                throw new CustomException(TerminalErrorCodes.EntityNotFound, "文章不存在");
            }
            // 非作者，且非管理员
            if (article.UserId != userId 
                && !await _userAppService.HasAnyRoleAsync(user?.Roles, RoleTypeEnum.Owner) 
                && !await _userAppService.HasAnyRoleAsync(user?.Roles, RoleTypeEnum.Admin))
            {
                throw new CustomException(TerminalErrorCodes.AuthorizeUserNotExistsOrForbidden, "无权限");
            }

            article.Title = input.Title;
            article.Content = input.Content;
            article.Summary = MarkdigHelper.GetSummary(input.Content, Constants.ArticleSummaryLength);
            article.UpdateTime = Clock.Now.ToMillisecondsTimestamp();

            // 保存标签
            var tags = await _tagRepository.GetOrCreateAsync((input.Tags ?? "").Split(","), Clock.Now.ToMillisecondsTimestamp());
            // 删除现有话题标签关系
            article!.ArticleTags?.ToList().ForEach(x =>
            {
                _articleTagRepository.DeleteAsync(x);
            });
            // 保存文章
            await _repository.UpdateAsync(article);
            // 保存文章标签关系
            await _articleTagRepository.AddArticleTagsAsync(article.Id, tags.Select(x => x.Id).ToArray(), Clock.Now.ToMillisecondsTimestamp());
            // 移除缓存
            await RemoveCacheAsync(id);
            return ObjectMapper.Map<Article, ArticleSimpleDto>(article);
        }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ArticleDto>> PostPagedAsync(SearchArticlePagedDto input)
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
            var articles = await _repository.GetListAsync(input.SkipCount, input.MaxResultCount, userIds.ToArray(), input.Title
                , input.Status.IsNullOrWhiteSpace() ? null : (StatusEnum)Enum.Parse(typeof(StatusEnum), input.Status!), true);

            var count = await _repository.GetCountAsync(userIds.ToArray(), input.Title
                , input.Status.IsNullOrWhiteSpace() ? null : (StatusEnum)Enum.Parse(typeof(StatusEnum), input.Status!));

            return new PagedResultDto<ArticleDto>(count, ObjectMapper.Map<List<Article>, List<ArticleDto>>(articles));
        }


        /// <summary>
        /// 修改文章标签
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<ArticleTagDto>> UpdateTagsAsync(long id, UpdateArticleTagDto input)
        {
            await _articleTagRepository.UpdateArticleTagAsync(id, 
                (input.TagIds ?? "")
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Convert.ToInt64(x))
                .ToArray(), Clock.Now.ToMillisecondsTimestamp());
            var tags = await _articleTagRepository.GetArticleTagsByArticleIdAsync(id, true);
            return ObjectMapper.Map<List<ArticleTag>, List<ArticleTagDto>>(tags ?? new List<ArticleTag>());
        }
    }
}
