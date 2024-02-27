using Volo.Abp.Domain.Repositories;

namespace ContIn.Abp.Terminal.Domain.Users
{
    /// <summary>
    /// 用户仓储接口
    /// </summary>
    public interface IUserRepository : IBasicRepository<User, long>
    {
        /// <summary>
        /// 根据用户名或者邮箱获取用户信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<User?> GetUserByNameOrEmailAsync(string name);
        /// <summary>
        /// 根据用户名获取信息
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<User?> GetUserByUserNameAsync(string username);
        /// <summary>
        /// 根据用户名模糊查询用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<List<User>?> FindUsersLikeUserNameAsync(string userName);
        /// <summary>
        /// 根据邮箱获取信息
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<User?> GetUserByEmailAsync(string email);
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="username"></param>
        /// <param name="nickname"></param>
        /// <returns></returns>
        Task<List<User>> GetListAsync(int skipCount, int maxResultCount, string? username, string? nickname);
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="username"></param>
        /// <param name="nickname"></param>
        /// <returns></returns>
        Task<int> GetCountAsync(string? username, string? nickname);
        /// <summary>
        /// 获取积分排行
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<List<User>> GetListByScoreAsync(int limit = 10);
        /// <summary>
        /// 设置背景图片
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        Task SetBackgroundImageAsync(long userId, string imageUrl);
        /// <summary>
        /// 设置用户头像
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        Task SetUserAvatarAsync(long userId, string imageUrl);
        /// <summary>
        /// 增加话题数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task IncrTopicCountAsync(long userId);
        /// <summary>
        /// 增加跟贴数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task IncrCommentCountAsync(long userId);
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
        /// 更新用户积分
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        Task UpdateUserScoreAsync(long userId, int score);
        /// <summary>
        /// 设置密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task UpdatePasswordAsync(long userId, string password);
        /// <summary>
        /// 设置用户名
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        Task UpdateUserNameAsync(long userId, string username);
        /// <summary>
        /// 设置邮箱
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        Task UpdateEmailAsync(long userId, string email);
    }
}
