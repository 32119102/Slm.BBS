using ContIn.Abp.Terminal.Application.Contracts.Users;
using ContIn.Abp.Terminal.Domain.Shared;
using ContIn.Abp.Terminal.Domain.Shared.Enum;
using ContIn.Abp.Terminal.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;

namespace ContIn.Abp.Terminal.Application.Users
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = SwaggerGroupName.User)]
    public class UserAppService : ApplicationService, IUserAppService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager _userManager;
        private readonly IUserScoreLogRepository _userScoreLogRepository;
        private readonly IDistributedCache<UserDto, long> _cache;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="userManager"></param>
        /// <param name="userScoreLogRepository">积分记录服务</param>
        /// <param name="cache"></param>
        public UserAppService(IUserRepository userRepository, UserManager userManager, IUserScoreLogRepository userScoreLogRepository
            , IDistributedCache<UserDto, long> cache)
        { 
            _userRepository = userRepository;
            _userManager = userManager;
            _cache = cache;
            _userScoreLogRepository = userScoreLogRepository;
        }

        /// <summary>
        /// 增加或者减少积分
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="score">积分</param>
        /// <param name="sourceType">来源类型</param>
        /// <param name="sourceId">来源编号</param>
        /// <param name="description">描述</param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task AddScoreAsync(long userId, int score, EntityTypeEnum sourceType, long sourceId, string description)
        {
            if (score == 0)
            {
                throw new CustomException(TerminalErrorCodes.CommonErrorCode, "积分不能为0");
            }
            var user = await GetAsync(userId);
            if (user == null)
            {
                throw new CustomException(TerminalErrorCodes.CommonErrorCode, "用户不存在");
            }
            // 更新用户积分
            await _userRepository.UpdateUserScoreAsync(userId, score);
            // 清楚用户缓存
            await _cache.RemoveAsync(userId);
            // 记录积分更改记录
            await _userScoreLogRepository.InsertAsync(new UserScoreLog()
            {
                Score = score,
                SourceId = sourceId.ToString(),
                SourceType = sourceType.ToString().ToLower(),
                CreateTime = Clock.Now.ToMillisecondsTimestamp(),
                Description = description,
                UserId = userId,
                Type = score > 0 ? ScoreTypeEnum.Incre : ScoreTypeEnum.Decre
            });
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<UserDto> CreateAsync(CreateUpdateUserDto input)
        {
            var user = await _userManager.CreateAsync(input.UserName!, input.Nickname!, input.Email!, input.Password, input.Roles, input.Status);
            var result = await _userRepository.InsertAsync(user);
            return ObjectMapper.Map<User, UserDto>(result);
        }

        /// <summary>
        /// 根据用户编号获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserDto?> GetAsync(long id)
        {
            return await _cache.GetOrAddAsync(id,
                async () =>
                {
                    var t = await _userRepository.FindAsync(id);
                    if (t == null)
                    {
                        // throw new CustomException(TerminalErrorCodes.EntityNotFound, "未查询到用户信息");
#pragma warning disable CS8603 // 可能返回 null 引用。
                        return null;
#pragma warning restore CS8603 // 可能返回 null 引用。
                    }
                    return ObjectMapper.Map<User, UserDto>(t);
                });
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        [AllowAnonymous]
        public async Task<UserSimpleDto?> GetSimpleUserAsync(long id)
        {
            var user = await _cache.GetAsync(id);
            if (user == null)
            {
                var t = await _userRepository.FindAsync(id);
                if (t == null)
                {
                    return null;
                }
                var v = ObjectMapper.Map<User, UserDto>(t);
                await _cache.SetAsync(id, v);
                return ObjectMapper.Map<UserDto, UserSimpleDto>(v);
            }
            return ObjectMapper.Map<UserDto, UserSimpleDto>(user);
        }

        /// <summary>
        /// 获取当前登录用户信息
        /// </summary>
        /// <param name="throwException">是否直接抛出异常</param>
        /// <returns>如果没有登录，返回空信息</returns>
        [AllowAnonymous]
        public async Task<UserDto?> GetCurrentAsync(bool throwException = false)
        {
            var id = CurrentUser.GetUserId();
            if (id <= 0)
            {
                if (throwException)
                {
                    throw new CustomException(TerminalErrorCodes.UserNotLogin, "用户未登录");
                }
                return null;
            }
            return await GetAsync(id);
        }

        /// <summary>
        /// 获取当前登录用户信息，如果没有返回null
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<UserProfileDto?> GetSiteCurrentAsync(bool throwException = false)
        {
            var id = CurrentUser.GetUserId();
            if (id <= 0)
            {
                if (throwException)
                {
                    throw new CustomException(TerminalErrorCodes.UserNotLogin, "用户未登录");
                }
                return null;
            }
            var user = await GetAsync(id);
            return user == null ? null : ObjectMapper.Map<UserDto, UserProfileDto>(user);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<UserProfileDto?> GetUserProfileAsync(long userId)
        {
            var user = await _cache.GetAsync(userId);
            if (user == null)
            {
                var t = await _userRepository.FindAsync(userId);
                if (t == null)
                {
                    return null;
                }
                var v = ObjectMapper.Map<User, UserDto>(t);
                await _cache.SetAsync(userId, v);
                return ObjectMapper.Map<UserDto, UserProfileDto>(v);
            }
            return ObjectMapper.Map<UserDto, UserProfileDto>(user);
        }

        /// <summary>
        /// 获取积分排行
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<List<UserSimpleDto>> GetScoreRankAsync()
        {
            var users = await _userRepository.GetListByScoreAsync();
            return ObjectMapper.Map<List<User>, List<UserSimpleDto>>(users);
        }

        /// <summary>
        /// 增加或者减少粉丝数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false)]
        public async Task IncrOrDecrFansCountAsync(long userId, int num)
        {
            await _userRepository.IncrOrDecrFansCountAsync(userId, num);
            await _cache.RemoveAsync(userId);
        }
        /// <summary>
        /// 增加或者减少关注数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false)]
        public async Task IncrOrDecrFollowsCountAsync(long userId, int num)
        {
            await _userRepository.IncrOrDecrFollowsCountAsync(userId, num);
            await _cache.RemoveAsync(userId);
        }

        /// <summary>
        /// 增加话题数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false)]
        public async Task IncrTopicCountAsync(long id)
        {
            await _userRepository.IncrTopicCountAsync(id);
            await _cache.RemoveAsync(id);
        }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<UserDto>> PostPagedAsync(GetPagedUserListDto input)
        {
            var users = await _userRepository.GetListAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.UserName,
                input.NickName
            );
            var totalCount = await _userRepository.GetCountAsync(input.UserName, input.NickName);
            return new PagedResultDto<UserDto>(
                totalCount,
                ObjectMapper.Map<List<User>, List<UserDto>>(users)
            );
        }

        /// <summary>
        /// 设置密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task PostSetPasswordAsync(SetUserPasswordDto input)
        {
            var userId = CurrentUser.GetUserId();
            var user = await GetAsync(userId);

            if (user == null)
            {
                throw new CustomException(TerminalErrorCodes.CommonErrorCode, "用户不存在");
            }
            if (!user.Password.IsNullOrWhiteSpace())
            {
                throw new CustomException(TerminalErrorCodes.CommonErrorCode, "你已设置了密码，如需修改请前往修改页面");
            }
            await _userRepository.UpdatePasswordAsync(userId, input.Password!);
            await _cache.RemoveAsync(userId);
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task PostUpdatePasswordAsync(UpdateUserPasswordDto input)
        {
            var userId = CurrentUser.GetUserId();
            var user = await GetAsync(userId);

            if (user == null)
            {
                throw new CustomException(TerminalErrorCodes.CommonErrorCode, "用户不存在");
            }
            if (user.Password.IsNullOrWhiteSpace())
            {
                throw new CustomException(TerminalErrorCodes.CommonErrorCode, "你没设置密码，请先设置密码");
            }
            if (!user.Password!.Equals(input.OldPassword, StringComparison.OrdinalIgnoreCase))
            {
                throw new CustomException(TerminalErrorCodes.CommonErrorCode, "旧密码验证失败");
            }
            if (user.Password!.Equals(input.Password, StringComparison.OrdinalIgnoreCase))
            {
                throw new CustomException(TerminalErrorCodes.CommonErrorCode, "新密码与旧密码一致，无需修改");
            }
            await _userRepository.UpdatePasswordAsync(userId, input.Password!);
            await _cache.RemoveAsync(userId);
        }

        /// <summary>
        /// 设置个人中心背景图片
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SetBackgroundImageAsync(SetBackgroundImageDto input)
        {
            var userId = CurrentUser.GetUserId();
            await _userRepository.SetBackgroundImageAsync(userId, input.Url!);
            await _cache.RemoveAsync(userId);
        }

        /// <summary>
        /// 设置用户名
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task SetUsernameAsync(SetUsernameDto input)
        {
            var userId = CurrentUser.GetUserId();
            var user = await GetAsync(userId);
            if(user != null && !string.IsNullOrWhiteSpace(user.UserName))
            {
                throw new CustomException(TerminalErrorCodes.CommonErrorCode, "你已设置了用户名，无法重复设置。");
            }
            var existing = await _userRepository.GetUserByUserNameAsync(input.Username!);
            if (existing != null)
            {
                throw new CustomException(TerminalErrorCodes.CommonErrorCode, "用户名：" + input.Username + " 已被占用");
            }
            await _userRepository.UpdateUserNameAsync(userId, input.Username!);
            await _cache.RemoveAsync(userId);
        }
        /// <summary>
        /// 设置邮箱
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task SetEmailAsync(SetUserEmailDto input)
        {
            var userId = CurrentUser.GetUserId();
            var user = await GetAsync(userId);
            if (user != null && user.Email!.Equals(input.Email, StringComparison.OrdinalIgnoreCase))
            {
                throw new CustomException(TerminalErrorCodes.CommonErrorCode, "和原邮箱一致，无需重复设置。");
            }
            var existing = await _userRepository.GetUserByEmailAsync(input.Email!);
            if (existing != null)
            {
                throw new CustomException(TerminalErrorCodes.CommonErrorCode, "邮箱：" + input.Email + " 已被占用");
            }
            await _userRepository.UpdateEmailAsync(userId, input.Email!);
            await _cache.RemoveAsync(userId);
        }

        /// <summary>
        /// 设置用户头像
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SetUserAvatarAsync(SetBackgroundImageDto input)
        {
            var userId = CurrentUser.GetUserId();
            await _userRepository.SetUserAvatarAsync(userId, input.Url!);
            await _cache.RemoveAsync(userId);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateAsync(long id, CreateUpdateUserDto input)
        {
            var t = await _userRepository.FindAsync(id);
            if (t == null)
            {
                throw new CustomException(TerminalErrorCodes.EntityNotFound, "没有找到此用户编号的信息");
            }

            if (!input.UserName!.Equals(t.UserName))
            {
                await _userManager.ChangeUserNameAsync(t, input.UserName);
            }
            if (!input.Email!.Equals(t.Email))
            { 
                await _userManager.ChangeEmailAsync(t, input.Email);
            }

            t.Nickname = input.Nickname;
            t.Status = input.Status;
            t.Roles = input.Roles;
            t.UpdateTime = Clock.Now.ToMillisecondsTimestamp();
            if (!input.Password.IsNullOrWhiteSpace())
            {
                t.Password = input.Password.ToMd5();
            }

            await _userRepository.UpdateAsync(t);
            await _cache.RemoveAsync(id);
        }

        /// <summary>
        /// 修改个人资料
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task UpdateUserInfoAsync(long id, UpdateUserInfoDto input)
        {
            var userId = CurrentUser.GetUserId();
            var user = await _userRepository.FindAsync(userId, false);
            if (user == null || user.Id != id)
            {
                throw new CustomException(TerminalErrorCodes.CommonErrorCode, "无权修改");
            }

            user.Nickname = input.NickName;
            user.Description = input.Description;
            user.HomePage = input.HomePage;

            await _userRepository.UpdateAsync(user);
            await _cache.RemoveAsync(id);
        }

        /// <summary>
        /// 校验用户状态
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task CheckUserStatusAsync(UserDto user)
        {
            _ = await _userManager.CheckUserStatusAsync(user.Status, user.ForbiddenEndTime);
        }
        /// <summary>
        /// 校验是否有角色
        /// </summary>
        /// <param name="userRoles"></param>
        /// <param name="roleType"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<bool> HasAnyRoleAsync(List<string>? userRoles, RoleTypeEnum roleType)
        {
            var result = _userManager.HasAnyRole(userRoles == null ? string.Empty : string.Join("", userRoles), roleType);
            return await Task.FromResult(result);
        }
    }
}
