using ContIn.Abp.Terminal.Domain.Shared.Enum;
using Volo.Abp.Application.Dtos;

namespace ContIn.Abp.Terminal.Application.Contracts.Users
{
    /// <summary>
    /// 用户传输对象
    /// </summary>
    public class UserDto : EntityDto<long>
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// 邮箱是否验证
        /// </summary>
        public int EmailVerified { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string? Nickname { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string? Password { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string? Avatar { get; set; }
        /// <summary>
        /// 个人中心背景图片
        /// </summary>
        public string? BackgroundImage { get; set; }
        /// <summary>
        /// 个人主页
        /// </summary>
        public string? HomePage { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public StatusEnum Status { get; set; }
        /// <summary>
        /// 帖子数量
        /// </summary>
        public int TopicCount { get; set; }
        /// <summary>
        /// 跟帖数量
        /// </summary>
        public int CommentCount { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public List<string>? Roles { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public UserTypeEnum Type { get; set; }
        /// <summary>
        /// 禁言结束时间
        /// </summary>
        public long ForbiddenEndTime { get; set; }
        /// <summary>
        /// 粉丝数量
        /// </summary>
        public int FansCount { get; set; }
        /// <summary>
        /// 关注数量
        /// </summary>
        public int FollowCount { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public long UpdateTime { get; set; }
    }
}
