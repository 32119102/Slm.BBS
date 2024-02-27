using ContIn.Abp.Terminal.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ContIn.Abp.Terminal.EntityFrameworkCore.Users
{
    public class UserRepository : EfCoreRepository<TerminalDbContext, User, long>, IUserRepository
    {
        public UserRepository(IDbContextProvider<TerminalDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        /// <summary>
        /// 根据用户名模糊查询
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<List<User>?> FindUsersLikeUserNameAsync(string userName)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.Where(x => x.UserName!.Contains(userName)).ToListAsync();
        }

        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="username"></param>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public async Task<int> GetCountAsync(string? username, string? nickname)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .WhereIf(!username.IsNullOrWhiteSpace(), x => x.UserName!.Contains(username!))
                .WhereIf(!nickname.IsNullOrWhiteSpace(), x => x.Nickname!.Contains(nickname!))
                .CountAsync();
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="username"></param>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public async Task<List<User>> GetListAsync(int skipCount, int maxResultCount, string? username, string? nickname)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .WhereIf(!username.IsNullOrWhiteSpace(), x => x.UserName!.Contains(username!))
                .WhereIf(!nickname.IsNullOrWhiteSpace(), x => x.Nickname!.Contains(nickname!))
                .OrderByDescending(x => x.Id)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }

        /// <summary>
        /// 获取积分排行
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<List<User>> GetListByScoreAsync(int limit = 10)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.OrderByDescending(x => x.Score)
                .Take(limit)
                .ToListAsync();
        }

        /// <summary>
        /// 根据邮箱获取用户
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(x => x.Email == email);
        }

        /// <summary>
        /// 根据用户名或者邮箱获取用户信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<User?> GetUserByNameOrEmailAsync(string name)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(x => x.UserName == name || x.Email == name);
        }
        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<User?> GetUserByUserNameAsync(string username)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(x => x.UserName == username);
        }

        /// <summary>
        /// 增加跟帖数量
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task IncrCommentCountAsync(long userId)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"update t_user set comment_count=comment_count+1 where id={userId}");
        }
        /// <summary>
        /// 增加粉丝数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public async Task IncrOrDecrFansCountAsync(long userId, int num)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"update t_user set fans_count=fans_count+{num} where id={userId}");
        }
        /// <summary>
        /// 增加关注数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public async Task IncrOrDecrFollowsCountAsync(long userId, int num)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"update t_user set follow_count=follow_count+{num} where id={userId}");
        }

        /// <summary>
        /// 增加话题数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task IncrTopicCountAsync(long userId)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"update t_user set topic_count=topic_count+1 where id={userId}");
        }

        /// <summary>
        /// 设置个人中心背景图片
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public async Task SetBackgroundImageAsync(long userId, string imageUrl)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"update t_user set background_image={imageUrl} where id={userId}");
        }
        /// <summary>
        /// 设置用户头像
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public async Task SetUserAvatarAsync(long userId, string imageUrl)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"update t_user set avatar={imageUrl} where id={userId}");
        }

        /// <summary>
        /// 设置密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task UpdatePasswordAsync(long userId, string password)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"update t_user set password={password} where id={userId}");
        }

        /// <summary>
        /// 设置用户名
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task UpdateUserNameAsync(long userId, string username)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"update t_user set username={username} where id={userId}");
        }
        /// <summary>
        /// 设置邮箱
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task UpdateEmailAsync(long userId, string email)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"update t_user set email={email} where id={userId}");
        }

        /// <summary>
        /// 更新用户积分
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public async Task UpdateUserScoreAsync(long userId, int score)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"update t_user set score=score+{score} where id={userId}");
        }
    }
}
