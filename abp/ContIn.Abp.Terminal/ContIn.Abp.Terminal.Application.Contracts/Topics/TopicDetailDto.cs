namespace ContIn.Abp.Terminal.Application.Contracts.Topics
{
    /// <summary>
    /// 话题详情
    /// </summary>
    public class TopicDetailDto : TopicSimpleDto
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string? Content { get; set; }
    }
}
