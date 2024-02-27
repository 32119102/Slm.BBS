using Volo.Abp.Application.Services;

namespace ContIn.Abp.Terminal.Application.Contracts.Favorites
{
    /// <summary>
    /// 收藏
    /// </summary>
    public interface IFavoriteAppService : IApplicationService
    {
        /// <summary>
        /// 获取用户收藏列表
        /// </summary>
        /// <param name="cursor"></param>
        /// <returns></returns>
        Task<SitePagedResultDto<UserFavoriteSimpleDto>> GetFavoritesAsync(long cursor = 0);
    }
}
