using ContIn.Abp.Terminal.Domain.Articles;
using ContIn.Abp.Terminal.Domain.Comments;
using ContIn.Abp.Terminal.Domain.Feed;
using ContIn.Abp.Terminal.Domain.Links;
using ContIn.Abp.Terminal.Domain.Sys;
using ContIn.Abp.Terminal.Domain.Tags;
using ContIn.Abp.Terminal.Domain.Topics;
using ContIn.Abp.Terminal.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace ContIn.Abp.Terminal.EntityFrameworkCore
{
    [ConnectionStringName("MySql")]
    public class TerminalDbContext : AbpDbContext<TerminalDbContext>
    {
        // 第三方账号
        public DbSet<ThirdAccount> ThirdAccounts { get; set; }
        // 用户
        public DbSet<User> Users { get; set; }
        // 用户积分记录
        public DbSet<UserScoreLog> UserScoreLogs { get; set; }
        // 标签
        public DbSet<Tag> Tags { get; set; }
        // 友情链接
        public DbSet<FriendlyLink> FriendlyLinks { get; set; }
        // 节点
        public DbSet<TopicNode> TopicNodes { get; set; }
        // 系统配置
        public DbSet<SysConfig> SysConfigs { get; set; }
        // 文章
        public DbSet<Article> Articles { get; set; }
        // 文章标签
        public DbSet<ArticleTag> ArticleTags { get; set; }
        // 签到
        public DbSet<CheckIn> CheckIns { get; set; }
        // 操作日志
        public DbSet<OperateLog> OperateLogs { get; set; }
        // 话题
        public DbSet<Topic> Topics { get; set; }
        // 话题标签
        public DbSet<TopicTag> TopicTags { get; set; }
        // 评论
        public DbSet<Comment> Comments { get; set; }
        // 消息
        public DbSet<Message> Messages { get; set; }
        // 关注粉丝
        public DbSet<UserFollow> UserFollows { get; set; }
        // 点赞
        public DbSet<UserLike> UserLikes { get; set; }
        // 收藏
        public DbSet<UserFavorite> UserFavorites { get; set; }
        /// <summary>
        /// Feed 流
        /// </summary>
        public DbSet<UserFeed> UserFeeds { get; set; }

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public TerminalDbContext(DbContextOptions<TerminalDbContext> options) : base(options)
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        {
        }

        public static readonly ILoggerFactory EfCoreLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(EfCoreLoggerFactory);
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureTag();
            modelBuilder.ConfigureLinks();
            modelBuilder.ConfigureTopicNode();
            modelBuilder.ConfigureUser();
            modelBuilder.ConfigureThirdAccount();
            modelBuilder.ConfigureUserScoreLog();
            modelBuilder.ConfigureSysConfig();
            modelBuilder.ConfigureArticle();
            modelBuilder.ConfigureArticleTag();
            modelBuilder.ConfigureCheckIn();
            modelBuilder.ConfigureOperateLog();
            modelBuilder.ConfigureTopic();
            modelBuilder.ConfigureTopicTag();
            modelBuilder.ConfigureComment();
            modelBuilder.ConfigureMessage();
            modelBuilder.ConfigureUserFollow();
            modelBuilder.ConfigureUserLike();
            modelBuilder.ConfigureUserFavorite();
            modelBuilder.ConfigureUserFeed();
        }
    }
}
