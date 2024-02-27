using ContIn.Abp.Terminal.Application.Contracts.Sys;
using ContIn.Abp.Terminal.Application.Contracts.Users;
using ContIn.Abp.Terminal.Application.LocalEventBus;
using ContIn.Abp.Terminal.Domain.Shared;
using ContIn.Abp.Terminal.Domain.Shared.Enum;
using ContIn.Abp.Terminal.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nito.AsyncEx;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.EventBus.Local;

namespace ContIn.Abp.Terminal.Application.Users
{
    /// <summary>
    /// 签到
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = SwaggerGroupName.User)]
    public class CheckInAppService : ApplicationService, ICheckInAppService
    {
        private readonly ICheckInRepository _repository;
        private readonly IUserAppService _userService;
        private readonly AsyncLock _mutex = new AsyncLock();
        private readonly ISysConfigAppService _sysConfigService;
        private readonly IDistributedCache<CheckInDto, long> _cache;
        private readonly string _rank_cache_key = "rank";
        private readonly IDistributedCache<List<CheckInDto>> _rankCache;
        private readonly ILocalEventBus _localEventBus;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="userService">用户服务</param>
        /// <param name="sysConfigAppService">系统配置服务</param>
        /// <param name="cache"></param>
        /// <param name="rankCache"></param>
        /// <param name="localEventBus">本地事件总线</param>
        public CheckInAppService(ICheckInRepository repository
            , IUserAppService userService
            , ISysConfigAppService sysConfigAppService
            , IDistributedCache<CheckInDto, long> cache
            , IDistributedCache<List<CheckInDto>> rankCache
            , ILocalEventBus localEventBus)
        { 
            _repository = repository;
            _userService = userService;
            _sysConfigService = sysConfigAppService;
            _cache = cache;
            _rankCache = rankCache;
            _localEventBus = localEventBus;
        }
        /// <summary>
        /// 获取用户签到信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<CheckInDto?> GetCheckInAsync(long userId)
        {
            var checkIn = await _cache.GetOrAddAsync(userId,
                async () =>
                {
                    var checkIn = await _repository.GetCheckInAsync(userId);
                    if (checkIn == null)
                    {
#pragma warning disable CS8603 // 可能返回 null 引用。
                        return null;
#pragma warning restore CS8603 // 可能返回 null 引用。
                    }
                    return ObjectMapper.Map<CheckIn, CheckInDto>(checkIn);
                });
            if (checkIn != null)
            {
                checkIn.CheckIn = Clock.Now.ToLongShortDate() == checkIn.DayName;
            }
            return checkIn;
        }

        /// <summary>
        /// 获取当前签到排行榜
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<List<CheckInDto>?> GetRankAsync()
        {
            var today = Clock.Now.ToLongShortDate();
            var key = $"{_rank_cache_key}:{today}";
            return await _rankCache.GetOrAddAsync(key,
                async () =>
                {
                    var list = await _repository.GetRankAsync(today);
                    return ObjectMapper.Map<List<CheckIn>, List<CheckInDto>>(list!);
                },
                () => new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions()
                {
                    AbsoluteExpiration = Clock.Now.AddDays(1)
                });
        }

        /// <summary>
        /// 用户签到
        /// </summary>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task PostCheckInAsync()
        {
            // 获取当前登录用户
            var user = await _userService.GetCurrentAsync(true);
            // 校验用户状态
            await _userService.CheckUserStatusAsync(user!);
            // 系统配置
            var config = await _sysConfigService.GetAllAsync();
            using (await _mutex.LockAsync())
            {
                var checkIn = await _repository.GetCheckInAsync(user!.Id);
                var dayName = Clock.Now.ToLongShortDate();
                if (checkIn != null && checkIn.LatestDayName == dayName)
                {
                    throw new CustomException(TerminalErrorCodes.CommonErrorCode, "你已签到");
                }
                long consecutiveDays = 1;
                if (checkIn != null && checkIn.LatestDayName == Clock.Now.AddDays(-1).ToLongShortDate())
                {
                    consecutiveDays = checkIn.ConsecutiveDays + 1;
                }
                if (checkIn == null)
                {
                    checkIn = new CheckIn()
                    {
                        UserId = user.Id,
                        CreateTime = Clock.Now.ToMillisecondsTimestamp(),
                    };
                }
                checkIn.LatestDayName = dayName;
                checkIn.ConsecutiveDays = consecutiveDays;
                checkIn.UpdateTime = Clock.Now.ToMillisecondsTimestamp();
                var checkInId = checkIn.Id;
                if (checkIn.Id == 0)
                {
                    var result = await _repository.InsertAsync(checkIn, true);
                    checkInId = result.Id;
                }
                else
                {
                    await _repository.UpdateAsync(checkIn);
                }
                // 发布签到积分事件
                await _localEventBus.PublishAsync(new UserScoreChangedEvent()
                {
                    UserId = user.Id,
                    SourceType = EntityTypeEnum.CheckIn,
                    SourceId = checkInId,
                    Description = "签到" + dayName,
                    Type = ScoreTypeEnum.Incre,
                    Score = config.ScoreConfig?.CheckInScore ?? 0
                });
                // 清理缓存
                await _cache.RemoveAsync(user.Id);
                await _rankCache.RemoveAsync($"{_rank_cache_key}:{Clock.Now.ToLongShortDate()}");
            }
        }
    }
}
