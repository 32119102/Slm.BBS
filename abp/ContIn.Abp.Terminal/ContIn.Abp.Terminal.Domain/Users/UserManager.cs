using ContIn.Abp.Terminal.Domain.Shared;
using ContIn.Abp.Terminal.Domain.Shared.Enum;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Timing;

namespace ContIn.Abp.Terminal.Domain.Users
{
    /// <summary>
    /// 用户领域服务
    /// </summary>
    public class UserManager : DomainService
    {
        private readonly IUserRepository _userRepository;
        private readonly IClock _clock;
        public UserManager(IUserRepository userRepository, IClock clock)
        { 
            _userRepository = userRepository;
            _clock = clock;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="username"></param>
        /// <param name="nickname"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="roles"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task<User> CreateAsync(string username, string nickname, string email, string? password, string? roles, StatusEnum status)
        {
            Check.NotNullOrWhiteSpace(username, nameof(username));
            Check.NotNullOrWhiteSpace(nickname, nameof(nickname));
            Check.NotNullOrWhiteSpace(email, nameof(email));
            if (password.IsNullOrWhiteSpace())
                password = "123456"; // 默认密码
            password = password.ToMd5();

            var existing = await _userRepository.GetUserByUserNameAsync(username);
            if (existing != null)
            {
                throw new CustomException(TerminalErrorCodes.EntityAlreadyExists, "此用户名已存在");
            }

            var existing2 = await _userRepository.GetUserByEmailAsync(email);
            if (existing2 != null)
            {
                throw new CustomException(TerminalErrorCodes.EntityAlreadyExists, "此邮箱已存在");
            }

            return new User
            {
                UserName = username,
                Nickname = nickname,
                Status = status,
                Email = email,
                Roles = roles,
                Password = password,
                CreateTime = _clock.Now.ToMillisecondsTimestamp(),
                UpdateTime = _clock.Now.ToMillisecondsTimestamp()
            };
        }
        /// <summary>
        /// 修改用户名
        /// </summary>
        /// <param name="user"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task ChangeUserNameAsync(User user, string newName)
        {
            Check.NotNull(user, nameof(user));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _userRepository.GetUserByUserNameAsync(newName);
            if (existing != null && existing.Id != user.Id)
            {
                throw new CustomException(TerminalErrorCodes.EntityAlreadyExists, "此用户名已存在");
            }
            user.UserName = newName;
        }
        /// <summary>
        /// 修改邮箱
        /// </summary>
        /// <param name="user"></param>
        /// <param name="newEmail"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task ChangeEmailAsync(User user, string newEmail)
        {
            Check.NotNull(user, nameof(user));
            Check.NotNullOrWhiteSpace(newEmail, nameof(newEmail));

            var existing = await _userRepository.GetUserByEmailAsync(newEmail);
            if (existing != null && existing.Id != user.Id)
            {
                throw new CustomException(TerminalErrorCodes.EntityAlreadyExists, "此邮箱已存在");
            }
            user.Email = newEmail;
        }

        /// <summary>
        /// 检查用户状态
        /// </summary>
        /// <param name="userStatus"></param>
        /// <param name="forbiddenEndTime"></param>
        /// <param name="checkForbidden">是否检查禁言状态</param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task<bool> CheckUserStatusAsync(StatusEnum userStatus, long forbiddenEndTime, bool checkForbidden = true)
        {
            // 状态
            if (userStatus != StatusEnum.Normal)
            {
                throw new CustomException(TerminalErrorCodes.AuthorizeUserNotExistsOrForbidden, "账号已禁用");
            }
            // 禁言
            if (checkForbidden && (forbiddenEndTime == -1 || forbiddenEndTime > _clock.Now.ToMillisecondsTimestamp()))
            {
                throw new CustomException(TerminalErrorCodes.AuthorizeUserNotExistsOrForbidden, "账号已禁言");
            }
            // 观察期

            return await Task.FromResult(true);
        }

        /// <summary>
        /// 检查有无权限
        /// </summary>
        /// <param name="userRoles"></param>
        /// <param name="roleType"></param>
        /// <returns></returns>
        public bool HasAnyRole(string? userRoles, RoleTypeEnum roleType)
        {
            if (userRoles.IsNullOrWhiteSpace() || (!(userRoles ?? string.Empty).Contains(roleType.ToString().ToLower())))
            {
                return false;
            }
            return true;
        }
    }
}
