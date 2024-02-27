using ContIn.Abp.Terminal.Application.Contracts;
using ContIn.Abp.Terminal.Application.Contracts.Articles;
using ContIn.Abp.Terminal.Application.Contracts.Favorites;
using ContIn.Abp.Terminal.Application.Contracts.Topics;
using ContIn.Abp.Terminal.Application.Contracts.Users;
using ContIn.Abp.Terminal.Domain.Shared;
using ContIn.Abp.Terminal.Domain.Shared.Enum;
using ContIn.Abp.Terminal.Domain.Shared.Options;
using ContIn.Abp.Terminal.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Services;

namespace ContIn.Abp.Terminal.Application.Favorites
{
    /// <summary>
    /// 收藏
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = SwaggerGroupName.User)]
    public class FavoriteAppService : ApplicationService, IFavoriteAppService
    {
        private readonly IUserFavoriteRepository _userFavoriteRepository;
        private readonly IUserAppService _userAppService;
        private readonly ITopicAppService _topicAppService;
        private readonly IArticleAppService _articleAppService;
        private readonly AppOptions _appOptions;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userFavoriteRepository"></param>
        /// <param name="userAppService">用户服务</param>
        /// <param name="topicAppService">话题服务</param>
        /// <param name="articleAppService">文章服务</param>
        /// <param name="appOptions"></param>
        public FavoriteAppService(IUserFavoriteRepository userFavoriteRepository, IUserAppService userAppService, ITopicAppService topicAppService, IArticleAppService articleAppService, IOptionsMonitor<AppOptions> appOptions)
        { 
            _userFavoriteRepository = userFavoriteRepository;
            _userAppService = userAppService;
            _topicAppService = topicAppService;
            _articleAppService = articleAppService;
            _appOptions = appOptions.CurrentValue;
        }
        /// <summary>
        /// 获取用户收藏列表
        /// </summary>
        /// <param name="cursor"></param>
        /// <returns></returns>
        public async Task<SitePagedResultDto<UserFavoriteSimpleDto>> GetFavoritesAsync(long cursor = 0)
        {
            int limit = 10;
            var userId = CurrentUser.GetUserId();
            var favorites = await _userFavoriteRepository.GetUserFavoritesAsync(userId, cursor, limit);

            List<UserFavoriteSimpleDto> list = ObjectMapper.Map<List<UserFavorite>, List<UserFavoriteSimpleDto>>(favorites);
            foreach (var f in list)
            {
                var user = await _userAppService.GetAsync(f.UserId);
                if (user != null)
                {
                    f.User = ObjectMapper.Map<UserDto, UserSimpleDto>(user);
                }
                if (f.EntityType!.Equals(EntityTypeEnum.Topic.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    var topic = await _topicAppService.GetSimpleAsync(f.EntityId);
                    if (topic == null || topic.Status == StatusEnum.Delete)
                    {
                        f.Deleted = true;
                    }
                    f.Title = topic?.Title;
                    f.Content = topic?.Summary;
                    f.Url = _appOptions.SiteBaseUrl + "/topic/" + f.EntityId;
                }
                else if (f.EntityType!.Equals(EntityTypeEnum.Article.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    var article = await _articleAppService.GetSimpleAsync(f.EntityId);
                    if (article == null || article.Status == StatusEnum.Delete)
                    {
                        f.Deleted = true;
                    }
                    f.Title = article?.Title;
                    f.Content = article?.Summary;
                    f.Url = _appOptions.SiteBaseUrl + "/article/" + f.EntityId;
                }
            }

            return new SitePagedResultDto<UserFavoriteSimpleDto>()
            {
                Cursor = favorites.Count > 0 ? favorites.Last().Id : cursor,
                HasMore = favorites.Count >= limit,
                Results = list
            };
        }
    }
}
