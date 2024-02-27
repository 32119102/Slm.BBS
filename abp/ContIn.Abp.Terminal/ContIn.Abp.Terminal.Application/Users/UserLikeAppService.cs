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
    /// 点赞
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = SwaggerGroupName.User)]
    public class UserLikeAppService : ApplicationService, IUserLikeAppService
    {
        // 仓储接口
        private readonly IUserLikeRepository _repository;
        private readonly IDistributedCache<List<UserLikeDto>> _cache;
        private readonly IDistributedCache<UserLikeDto> _likedCache;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="cache"></param>
        /// <param name="likedCache"></param>
        public UserLikeAppService(IUserLikeRepository repository, IDistributedCache<List<UserLikeDto>> cache, IDistributedCache<UserLikeDto> likedCache)
        {
            _repository = repository;
            _cache = cache;
            _likedCache = likedCache;
        }

        /// <summary>
        /// 是否点赞
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="entityId">实体编号</param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<bool> GetLikedAsync(string entityType, long entityId)
        {
            var userId = CurrentUser.GetUserId();
            if (userId <= 0)
            {
                return false;
            }
            var key = $"liked:{userId}:{entityType.ToLower()}:{entityId}";
            return await _likedCache.GetOrAddAsync(key,
                async () =>
                {
                    var result = await _repository.GetLikedAsync(userId, entityType, entityId);
#pragma warning disable CS8603 // 可能返回 null 引用。
                    return result != null ? ObjectMapper.Map<UserLike, UserLikeDto>(result) : null;
#pragma warning restore CS8603 // 可能返回 null 引用。
                }) != null;
        }


        /// <summary>
        /// 获取某个实体最近点赞
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="entityId">实体编号</param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [RemoteService(false)]
        [AllowAnonymous]
        public async Task<List<UserLikeDto>> GetRecentAsync(string entityType, long entityId, int limit = 5)
        {
            var key = $"{entityType.ToLower()}:{entityId}";
            return await _cache.GetOrAddAsync(key,
                async () =>
                {
                    var list = await _repository.GetRecentAsync(entityType, entityId, limit, true);
                    if (list == null || list.Count == 0)
                        return new List<UserLikeDto>();
                    return ObjectMapper.Map<List<UserLike>, List<UserLikeDto>>(list);
                });
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<UserLikeDto> InsertAsync(CreateUserLikeDto input)
        {
            var existing = await _repository.GetLikedAsync(input.UserId, input.EntityType!, input.EntityId);
            if (existing == null)
            {
                var result = await _repository.InsertAsync(new UserLike()
                {
                    UserId = input.UserId,
                    EntityType = input.EntityType!.ToString().ToLower(),
                    EntityId = input.EntityId,
                    CreateTime = Clock.Now.ToMillisecondsTimestamp(),
                });
                // 删除缓存
                var key = $"{input.EntityType.ToLower()}:{input.EntityId}";
                await _cache.RemoveAsync(key);

                var keyd = $"liked:{input.UserId}:{input.EntityType.ToLower()}:{input.EntityId}";
                await _likedCache.RemoveAsync(keyd);
                
                return ObjectMapper.Map<UserLike, UserLikeDto>(result);
            }
            return ObjectMapper.Map<UserLike, UserLikeDto>(existing);
        }
    }
}
