using ContIn.Abp.Terminal.Domain.Shared.Enum;
using Volo.Abp.Domain.Entities;

namespace ContIn.Abp.Terminal.Domain.Users
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User : Entity<long>
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public virtual string? UserName { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public virtual string? Email { get; set; }
        /// <summary>
        /// 邮箱是否验证
        /// </summary>
        public virtual int EmailVerified { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public virtual string? Nickname { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public virtual string? Avatar { get; set; }
        /// <summary>
        /// 个人中心背景图片
        /// </summary>
        public virtual string? BackgroundImage { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public virtual string? Password { get; set; }
        /// <summary>
        /// 个人主页
        /// </summary>
        public virtual string? HomePage { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public virtual string? Description { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public virtual int Score { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public virtual StatusEnum Status { get; set; }
        /// <summary>
        /// 帖子数量
        /// </summary>
        public virtual int TopicCount { get; set; }
        /// <summary>
        /// 跟帖数量
        /// </summary>
        public virtual int CommentCount { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public virtual string? Roles { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public virtual UserTypeEnum Type { get; set; }
        /// <summary>
        /// 禁言结束时间
        /// </summary>
        public virtual long ForbiddenEndTime { get; set; }
        /// <summary>
        /// 粉丝数量
        /// </summary>
        public virtual int FansCount { get; set; }
        /// <summary>
        /// 关注数量
        /// </summary>
        public virtual int FollowCount { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual long CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual long UpdateTime { get; set; }
    }
}
