using ContIn.Abp.Terminal.Application.Contracts.Feed;
using ContIn.Abp.Terminal.Application.Contracts.Follows;
using ContIn.Abp.Terminal.Application.Contracts.Users;
using ContIn.Abp.Terminal.Domain.Feed;
using ContIn.Abp.Terminal.Domain.Shared;
using ContIn.Abp.Terminal.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;

namespace ContIn.Abp.Terminal.Application.Follows
{
    /// <summary>
    /// 粉丝关注
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = SwaggerGroupName.User)]
    public class FollowAppService : ApplicationService, IFollowAppService
    {
        private readonly IUserFollowRepository _repository;
        private readonly IUserAppService _userService;
        private readonly IUserFeedRepository _userFeedRepository;
        private readonly IDistributedCache<UserFollowDto> _cache;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="userService">用户服务</param>
        /// <param name="userFeedRepository"></param>
        /// <param name="cache"></param>
        public FollowAppService(IUserFollowRepository repository, IUserAppService userService, IUserFeedRepository userFeedRepository, IDistributedCache<UserFollowDto> cache)
        {
            _repository = repository;
            _userService = userService;
            _userFeedRepository = userFeedRepository;
            _cache = cache;
        }

        /// <summary>
        /// 获取粉丝列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cursor"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<UserFollowFansDto> GetFansAsync(long userId, long cursor = 0, int limit = 5)
        {
            var fans = await _repository.GetFansAsync(userId, cursor, limit, true);
            // var currentUser = await _userService.GetSiteCurrentAsync();

            UserFollowFansDto dto = new UserFollowFansDto()
            {
                Cursor = fans.Count > 0 ? fans.Last().Id : 0,
                HasMore = fans.Count >= limit
            };
            var userDtos = ObjectMapper.Map<List<User>, List<UserSimpleDto>>(fans.Select(x => x.User!).ToList());
            userDtos.ForEach(x =>
            {
                // x.Followed = _repository.IsFollowedAsync(currentUser!.Id, x.Id).Result;
                x.Followed = IsFollowedAsync(x.Id).Result;
            });
            dto.Results = userDtos;
            return dto;
        }

        /// <summary>
        /// 获取关注列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cursor"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<UserFollowFansDto> GetFollowsAsync(long userId, long cursor = 0, int limit = 5)
        {
            var follows = await _repository.GetFollowsAsync(userId, cursor, limit, true);
            // var currentUser = await _userService.GetSiteCurrentAsync();

            UserFollowFansDto dto = new UserFollowFansDto()
            { 
                Cursor = follows.Count > 0 ? follows.Last().Id : 0,
                HasMore = follows.Count >= limit
            };
            var userDtos = ObjectMapper.Map<List<User>, List<UserSimpleDto>>(follows.Select(x => x.Other!).ToList());
            userDtos.ForEach(x =>
            {
                // x.Followed = await _repository.IsFollowedAsync(currentUser!.Id, x.Id);
                x.Followed = IsFollowedAsync(x.Id).Result;
            });
            dto.Results = userDtos;
            return dto;
        }

        /// <summary>
        /// 是否关注
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<bool> IsFollowedAsync(long userId)
        {
            // var currentUser = await _userService.GetSiteCurrentAsync();
            var currUserId = CurrentUser.GetUserId();
            if (currUserId == 0)
            {
                return false;
            }
            // return await _repository.IsFollowedAsync(currentUser!.Id, userId);
            return await GetUserFollowedAsync(currUserId, userId) != null;
        }

        /// <summary>
        /// 是否关注，有缓存
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="otherUserId"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<UserFollowDto> GetUserFollowedAsync(long userId, long otherUserId)
        {
            var key = $"{userId},{otherUserId}";
            return await _cache.GetOrAddAsync(key,
                async () =>
                {
                    var follow = await _repository.GetUserFollowAsync(userId, otherUserId);
#pragma warning disable CS8603 // 可能返回 null 引用。
                    return follow == null ? null : ObjectMapper.Map<UserFollow, UserFollowDto>(follow);
#pragma warning restore CS8603 // 可能返回 null 引用。
                });
        }

        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task PostFollowAsync(UserFollowUpdateDto input)
        {
            var userId = CurrentUser.GetUserId();
            if (userId == input.UserId)
            {
                throw new CustomException(TerminalErrorCodes.CommonErrorCode, "不能关注自己");
            }
            // 判断是否已经关注
            if (await IsFollowedAsync(input.UserId))
            {
                throw new CustomException(TerminalErrorCodes.CommonErrorCode, "已关注");
            }
            await _repository.InsertAsync(new UserFollow()
            {
                UserId = userId,
                OtherId = input.UserId,
                Status = Domain.Shared.Enum.StatusEnum.Normal,
                CreateTime = Clock.Now.ToMillisecondsTimestamp()
            });
            // 增加对方粉丝数
            await _userService.IncrOrDecrFansCountAsync(input.UserId, 1);
            // 增加自己关注数
            await _userService.IncrOrDecrFollowsCountAsync(userId, 1);
        }

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task PostUnFollowAsync(UserFollowUpdateDto input)
        {
            var userId = CurrentUser.GetUserId();
            if (userId == input.UserId)
            {
                throw new CustomException(TerminalErrorCodes.CommonErrorCode, "不能取消关注自己");
            }
            // 判断是否已经取消关注
            if (!await IsFollowedAsync(input.UserId))
            {
                throw new CustomException(TerminalErrorCodes.CommonErrorCode, "已取消关注");
            }
            var follow = await _repository.GetUserFollowAsync(userId, input.UserId);
            if (follow != null)
            {
                await _repository.DeleteAsync(follow.Id);
            }
            // 移除缓存
            _cache.Remove($"{userId},{input.UserId}");
            // 减少对方粉丝数
            await _userService.IncrOrDecrFansCountAsync(input.UserId, -1);
            // 减少自己关注数
            await _userService.IncrOrDecrFollowsCountAsync(userId, -1);
            // 取消Feed流
            await _userFeedRepository.DeleteByUserAsync(userId, input.UserId);
        }
    }
}
