using ContIn.Abp.Terminal.Domain.Shared.Enum;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ContIn.Abp.Terminal.Application.Contracts.Users
{
    /// <summary>
    /// 用户
    /// </summary>
    public interface IUserAppService : IApplicationService
    {
        /// <summary>
        /// 获取当前登录用户信息
        /// </summary>
        /// <param name="throwException">是否直接抛出异常</param>
        /// <returns></returns>
        Task<UserDto?> GetCurrentAsync(bool throwException = false);
        /// <summary>
        /// 获取当前登录用户信息，如果没有返回null
        /// </summary>
        /// <param name="throwException">是否直接抛出异常</param>
        /// <returns></returns>
        Task<UserProfileDto?> GetSiteCurrentAsync(bool throwException = false);
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserProfileDto?> GetUserProfileAsync(long userId);
        /// <summary>
        /// 根据用户编号获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserDto?> GetAsync(long id);
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserSimpleDto?> GetSimpleUserAsync(long id);
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<UserDto>> PostPagedAsync(GetPagedUserListDto input);
        /// <summary>
        /// 获取积分排行
        /// </summary>
        /// <returns></returns>
        Task<List<UserSimpleDto>> GetScoreRankAsync();
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<UserDto> CreateAsync(CreateUpdateUserDto input);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateAsync(long id, CreateUpdateUserDto input);
        /// <summary>
        /// 设置背景图片
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SetBackgroundImageAsync(SetBackgroundImageDto input);
        /// <summary>
        /// 设置用户头像
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SetUserAvatarAsync(SetBackgroundImageDto input);
        /// <summary>
        /// 个人资料修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateUserInfoAsync(long id, UpdateUserInfoDto input);
        /// <summary>
        /// 增加话题数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task IncrTopicCountAsync(long id);
        /// <summary>
        /// 增加粉丝数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        Task IncrOrDecrFansCountAsync(long userId, int num);
        /// <summary>
        /// 增加关注数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        Task IncrOrDecrFollowsCountAsync(long userId, int num);

        /// <summary>
        /// 增加或者减少积分
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="score">分数</param>
        /// <param name="sourceType">来源类型</param>
        /// <param name="sourceId">来源编号</param>
        /// <param name="description">描述</param>
        /// <returns></returns>
        Task AddScoreAsync(long userId, int score, EntityTypeEnum sourceType, long sourceId, string description);

        /// <summary>
        /// 设置密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task PostSetPasswordAsync(SetUserPasswordDto input);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task PostUpdatePasswordAsync(UpdateUserPasswordDto input);

        /// <summary>
        /// 设置用户名
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SetUsernameAsync(SetUsernameDto input);
        /// <summary>
        /// 设置邮箱
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SetEmailAsync(SetUserEmailDto input);

        /// <summary>
        /// 校验用户状态
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task CheckUserStatusAsync(UserDto user);
        /// <summary>
        /// 检查是否有权限
        /// </summary>
        /// <param name="userRoles"></param>
        /// <param name="roleType"></param>
        /// <returns></returns>
        Task<bool> HasAnyRoleAsync(List<string>? userRoles, RoleTypeEnum roleType);
    }
}
