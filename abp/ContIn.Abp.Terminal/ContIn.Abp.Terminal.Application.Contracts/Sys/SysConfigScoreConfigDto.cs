namespace ContIn.Abp.Terminal.Application.Contracts.Sys
{
    /// <summary>
    /// 积分配置
    /// </summary>
    public class SysConfigScoreConfigDto
    {
        /// <summary>
        /// 发帖积分
        /// </summary>
        public int PostTopicScore { get; set; }
        /// <summary>
        /// 跟帖积分
        /// </summary>
        public int PostCommentScore { get; set; }
        /// <summary>
        /// 签到积分
        /// </summary>
        public int CheckInScore { get; set; }
    }
}
