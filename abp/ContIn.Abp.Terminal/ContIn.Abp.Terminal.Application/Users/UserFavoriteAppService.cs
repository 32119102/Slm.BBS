using ContIn.Abp.Terminal.Application.Contracts.Users;
using ContIn.Abp.Terminal.Domain.Shared;
using ContIn.Abp.Terminal.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;

namespace ContIn.Abp.Terminal.Application.Users
{
    /// <summary>
    /// 收藏
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = SwaggerGroupName.User)]
    public class UserFavoriteAppService : ApplicationService, IUserFavoriteAppService
    {
        private readonly IUserFavoriteRepository _repository;
        private readonly IDistributedCache<UserFavoriteDto> _cache;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="cache"></param>
        public UserFavoriteAppService(IUserFavoriteRepository repository, IDistributedCache<UserFavoriteDto> cache)
        { 
            _repository = repository;
            _cache = cache;
        }

        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public async Task DeletedAsync(string entityType, long entityId)
        {
            var userId = CurrentUser.GetUserId();
            await _repository.DeletedAsync(userId, entityType, entityId);
            // 移除缓存
            var key = $"favorite:{userId}:{entityType.ToLower()}:{entityId}";
            await _cache.RemoveAsync(key);
        }

        /// <summary>
        /// 是否收藏
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<bool> GetFavoritedAsync(string entityType, long entityId)
        {
            var userId = CurrentUser.GetUserId();
            if (userId <= 0)
            {
                return false;
            }
            var key = $"favorite:{userId}:{entityType.ToLower()}:{entityId}";
            return await _cache.GetOrAddAsync(key,
                async () =>
                {
                    var userFavorite = await _repository.GetUserFavoriteAsync(userId, entityType, entityId);
                    if (userFavorite == null)
                    {
#pragma warning disable CS8603 // 可能返回 null 引用。
                        return null;
#pragma warning restore CS8603 // 可能返回 null 引用。
                    }
                    return ObjectMapper.Map<UserFavorite, UserFavoriteDto>(userFavorite);
                }) != null;
        }

        /// <summary>
        /// 新增收藏
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entityType"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<UserFavoriteDto> InsertAsync(long userId, string entityType, long entityId)
        {
            var existing = await _repository.GetUserFavoriteAsync(userId, entityType, entityId);
            if (existing == null)
            {
                var userFavorate = await _repository.InsertAsync(new UserFavorite()
                {
                    UserId = userId,
                    EntityType = entityType.ToLower(),
                    EntityId = entityId,
                    CreateTime = Clock.Now.ToMillisecondsTimestamp()
                });
                // 移除缓存
                var key = $"favorite:{userId}:{entityType.ToLower()}:{entityId}";
                await _cache.RemoveAsync(key);
                return ObjectMapper.Map<UserFavorite, UserFavoriteDto>(userFavorate);
            }
            return ObjectMapper.Map<UserFavorite, UserFavoriteDto>(existing);
        }
    }
}
