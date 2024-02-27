namespace ContIn.Abp.Terminal.Application.Contracts.Topics
{
    /// <summary>
    /// 话题
    /// </summary>
    public class TopicPagedResultDto
    {
        /// <summary>
        /// 游标，上一个话题的编号
        /// </summary>
        public long Cursor { get; set; }
        /// <summary>
        /// 是否还有更多
        /// </summary>
        public bool HasMore { get; set; }
        /// <summary>
        /// 话题列表
        /// </summary>
        public List<TopicSimpleDto>? Results { get; set; }
    }
}
