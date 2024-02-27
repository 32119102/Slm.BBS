namespace ContIn.Abp.Terminal.Application.Contracts.Users
{
    /// <summary>
    /// 简要用户信息
    /// </summary>
    public class UserSimpleDto
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string? Nickname { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string? Avatar { get; set; }
        /// <summary>
        /// 头像小图
        /// </summary>
        public string? SmallAvatar { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 帖子数量
        /// </summary>
        public int TopicCount { get; set; }
        /// <summary>
        /// 跟帖数量
        /// </summary>
        public int CommentCount { get; set; }
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
        /// 是否关注
        /// </summary>
        public bool Followed { get; set; }
    }
}
