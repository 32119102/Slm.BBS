using ContIn.Abp.Terminal.Domain.Articles;
using ContIn.Abp.Terminal.Domain.Comments;
using ContIn.Abp.Terminal.Domain.Feed;
using ContIn.Abp.Terminal.Domain.Links;
using ContIn.Abp.Terminal.Domain.Sys;
using ContIn.Abp.Terminal.Domain.Tags;
using ContIn.Abp.Terminal.Domain.Topics;
using ContIn.Abp.Terminal.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ContIn.Abp.Terminal.EntityFrameworkCore
{
    public static class TerminalModelBuilderExtensions
    {
        /// <summary>
        /// Feed流
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureUserFeed(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));
            builder.Entity<UserFeed>(b =>
            {
                b.ToTable("t_user_feed");
                b.ConfigureByConvention();
                b.Property(x => x.UserId).HasColumnName("user_id");
                b.Property(x => x.DataType).HasColumnName("data_type");
                b.Property(x => x.DataId).HasColumnName("data_id");
                b.Property(x => x.AuthorId).HasColumnName("author_id");
                b.Property(x => x.CreateTime).HasColumnName("create_time");
            });
        }

        /// <summary>
        /// 收藏
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureUserFavorite(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));
            builder.Entity<UserFavorite>(b =>
            {
                b.ToTable("t_favorite");
                b.ConfigureByConvention();
                b.Property(x => x.UserId).HasColumnName("user_id");
                b.Property(x => x.EntityType).HasColumnName("entity_type");
                b.Property(x => x.EntityId).HasColumnName("entity_id");
                b.Property(x => x.CreateTime).HasColumnName("create_time");
            });
        }

        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureUserLike(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));
            builder.Entity<UserLike>(b =>
            {
                b.ToTable("t_user_like");
                b.ConfigureByConvention();
                b.Property(x => x.UserId).HasColumnName("user_id");
                b.Property(x => x.EntityType).HasColumnName("entity_type");
                b.Property(x => x.EntityId).HasColumnName("entity_id");
                b.Property(x => x.CreateTime).HasColumnName("create_time");
            });
        }

        /// <summary>
        /// 关注粉丝
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureUserFollow(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));
            builder.Entity<UserFollow>(b =>
            {
                b.ToTable("t_user_follow");
                b.ConfigureByConvention();
                b.Property(x => x.UserId).HasColumnName("user_id");
                b.Property(x => x.OtherId).HasColumnName("other_id");
                b.Property(x => x.CreateTime).HasColumnName("create_time");
            });
        }

        /// <summary>
        /// 消息
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureMessage(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));
            builder.Entity<Message>(b =>
            {
                b.ToTable("t_message");
                b.ConfigureByConvention();
                b.Property(x => x.FromId).HasColumnName("from_id");
                b.Property(x => x.UserId).HasColumnName("user_id");
                b.Property(x => x.QuoteContent).HasColumnName("quote_content");
                b.Property(x => x.ExtraData).HasColumnName("extra_data");
                b.Property(x => x.CreateTime).HasColumnName("create_time");
            });
        }

        /// <summary>
        /// 评论
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureComment(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<Comment>(b =>
            {
                b.ToTable("t_comment");

                b.ConfigureByConvention();

                b.Property(x => x.UserId).HasColumnName("user_id");
                b.Property(x => x.EntityType).HasColumnName("entity_type");
                b.Property(x => x.EntityId).HasColumnName("entity_id");
                b.Property(x => x.ImageList).HasColumnName("image_list");
                b.Property(x => x.ContentType).HasColumnName("content_type");
                b.Property(x => x.QuoteId).HasColumnName("quote_id");
                b.Property(x => x.LikeCount).HasColumnName("like_count");
                b.Property(x => x.CommentCount).HasColumnName("comment_count");
                b.Property(x => x.UserAgent).HasColumnName("user_agent");
                b.Property(x => x.CreateTime).HasColumnName("create_time");
            });
        }
        /// <summary>
        /// 话题标签
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureTopicTag(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<TopicTag>(b =>
            {
                b.ToTable("t_topic_tag");

                b.ConfigureByConvention();

                b.Property(x => x.TopicId).HasColumnName("topic_id");
                b.Property(x => x.TagId).HasColumnName("tag_id");
                b.Property(x => x.LastCommentTime).HasColumnName("last_comment_time");
                b.Property(x => x.LastCommentUserId).HasColumnName("last_comment_user_id");
                b.Property(x => x.CreateTime).HasColumnName("create_time");
            });
        }
        /// <summary>
        /// 话题
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureTopic(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<Topic>(b =>
            {
                b.ToTable("t_topic");

                b.ConfigureByConvention();

                b.Property(x => x.NodeId).HasColumnName("node_id");
                b.Property(x => x.UserId).HasColumnName("user_id");
                b.Property(x => x.ImageList).HasColumnName("image_list");
                b.Property(x => x.RecommendTime).HasColumnName("recommend_time");
                b.Property(x => x.ViewCount).HasColumnName("view_count");
                b.Property(x => x.CommentCount).HasColumnName("comment_count");
                b.Property(x => x.LikeCount).HasColumnName("like_count");
                b.Property(x => x.LastCommentTime).HasColumnName("last_comment_time");
                b.Property(x => x.LastCommentUserId).HasColumnName("last_comment_user_id");
                b.Property(x => x.UserAgent).HasColumnName("user_agent");
                b.Property(x => x.CreateTime).HasColumnName("create_time");
                b.Property(x => x.ExtraData).HasColumnName("extra_data");
                b.Property(x => x.StickyTime).HasColumnName("sticky_time");
            });
        }
        /// <summary>
        /// 操作日志
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureOperateLog(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<OperateLog>(b =>
            {
                b.ToTable("t_operate_log");

                b.ConfigureByConvention();

                b.Property(x => x.UserId).HasColumnName("user_id");
                b.Property(x => x.OpType).HasColumnName("op_type");
                b.Property(x => x.DataType).HasColumnName("data_type");
                b.Property(x => x.DataId).HasColumnName("data_id");
                b.Property(x => x.UserAgent).HasColumnName("user_agent");
                b.Property(x => x.CreateTime).HasColumnName("create_time");
            });
        }
        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureCheckIn(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<CheckIn>(b =>
            {
                b.ToTable("t_check_in");

                b.ConfigureByConvention();

                b.Property(x => x.UserId).HasColumnName("user_id");
                b.Property(x => x.LatestDayName).HasColumnName("latest_day_name");
                b.Property(x => x.ConsecutiveDays).HasColumnName("consecutive_days");
                b.Property(x => x.CreateTime).HasColumnName("create_time");
                b.Property(x => x.UpdateTime).HasColumnName("update_time");
            });
        }
        /// <summary>
        /// 文章标签
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureArticleTag(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<ArticleTag>(b =>
            {
                b.ToTable("t_article_tag");

                b.ConfigureByConvention();

                b.Property(x => x.ArticleId).HasColumnName("article_id");
                b.Property(x => x.TagId).HasColumnName("tag_id");
                b.Property(x => x.CreateTime).HasColumnName("create_time");
            });
        }
        /// <summary>
        /// 文章
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureArticle(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<Article>(b =>
            {
                b.ToTable("t_article");

                b.ConfigureByConvention();

                b.Property(x => x.UserId).HasColumnName("user_id");
                b.Property(x => x.ContentType).HasColumnName("content_type");
                b.Property(x => x.SourceUrl).HasColumnName("source_url");
                b.Property(x => x.ViewCount).HasColumnName("view_count");
                b.Property(x => x.CreateTime).HasColumnName("create_time");
                b.Property(x => x.UpdateTime).HasColumnName("update_time");
            });
        }

        /// <summary>
        /// 用户积分记录
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureUserScoreLog(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<UserScoreLog>(b =>
            {
                b.ToTable("t_user_score_log");

                b.ConfigureByConvention();

                b.Property(x => x.UserId).HasColumnName("user_id");
                b.Property(x => x.SourceType).HasColumnName("source_type");
                b.Property(x => x.SourceId).HasColumnName("source_id");
                b.Property(x => x.CreateTime).HasColumnName("create_time");
            });
        }

        /// <summary>
        /// 系统配置
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureSysConfig(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<SysConfig>(b =>
            {
                b.ToTable("t_sys_config");

                b.ConfigureByConvention();

                b.Property(x => x.CreateTime).HasColumnName("create_time");
                b.Property(x => x.UpdateTime).HasColumnName("update_time");
            });
        }

        /// <summary>
        /// 节点
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureTopicNode(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<TopicNode>(b =>
            {
                b.ToTable("t_topic_node");

                b.ConfigureByConvention();

                b.Property(x => x.SortNo).HasColumnName("sort_no");
                b.Property(x => x.CreateTime).HasColumnName("create_time");
            });
        }

        /// <summary>
        /// 友情链接
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureLinks(this ModelBuilder builder)
        { 
            Check.NotNull(builder, nameof(builder));

            builder.Entity<FriendlyLink>(b =>
            {
                b.ToTable("t_link");

                b.ConfigureByConvention();

                b.Property(x => x.CreateTime).HasColumnName("create_time");
            });
        }

        /// <summary>
        /// 标签
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureTag(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<Tag>(b =>
            {
                b.ToTable("t_tag");

                b.ConfigureByConvention();

                b.Property(x => x.CreateTime).HasColumnName("create_time");
                b.Property(x => x.UpdateTime).HasColumnName("update_time");
            });
        }
        /// <summary>
        /// 第三方账号
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureThirdAccount(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<ThirdAccount>(b =>
            {
                b.ToTable("t_third_account");

                b.ConfigureByConvention();

                b.Property(x => x.UserId).HasColumnName("user_id");
                b.Property(x => x.ThirdType).HasColumnName("third_type");
                b.Property(x => x.ThirdId).HasColumnName("third_id");
                b.Property(x => x.ExtraData).HasColumnName("extra_data");
                b.Property(x => x.CreateTime).HasColumnName("create_time");
                b.Property(x => x.UpdateTime).HasColumnName("update_time");
            });
        }
        /// <summary>
        /// 用户
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureUser(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<User>(b =>
            {
                b.ToTable("t_user");

                b.ConfigureByConvention();

                b.Property(x => x.EmailVerified).HasColumnName("email_verified");
                b.Property(x => x.BackgroundImage).HasColumnName("background_image");
                b.Property(x => x.HomePage).HasColumnName("home_page");
                b.Property(x => x.TopicCount).HasColumnName("topic_count");
                b.Property(x => x.CommentCount).HasColumnName("comment_count");
                b.Property(x => x.ForbiddenEndTime).HasColumnName("forbidden_end_time");
                b.Property(x => x.CreateTime).HasColumnName("create_time");
                b.Property(x => x.UpdateTime).HasColumnName("update_time");
                b.Property(x => x.FansCount).HasColumnName("fans_count");
                b.Property(x => x.FollowCount).HasColumnName("follow_count");
            });
        }
    }
}
