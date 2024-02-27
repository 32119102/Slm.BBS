namespace ContIn.Abp.Terminal.Application.Contracts.Follows
{
    /// <summary>
    /// 关注和取消关注
    /// </summary>
    public class UserFollowUpdateDto
    {
        /// <summary>
        /// 对方用户编号
        /// </summary>
        public long UserId { get; set; }
    }
}
