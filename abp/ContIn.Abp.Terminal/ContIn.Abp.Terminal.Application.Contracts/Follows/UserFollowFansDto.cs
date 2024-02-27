using ContIn.Abp.Terminal.Application.Contracts.Users;

namespace ContIn.Abp.Terminal.Application.Contracts.Follows
{
    /// <summary>
    /// 关注粉丝
    /// </summary>
    public class UserFollowFansDto
    {
        /// <summary>
        /// 游标，上一条记录的编号
        /// </summary>
        public long Cursor { get; set; }
        /// <summary>
        /// 是否还有更多
        /// </summary>
        public bool HasMore { get; set; }
        /// <summary>
        /// 用户列表
        /// </summary>
        public List<UserSimpleDto>? Results { get; set; }
    }
}
