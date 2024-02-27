namespace ContIn.Abp.Terminal.Application.Contracts.Topics
{
    /// <summary>
    /// 节点信息
    /// </summary>
    public class TopicNodeSimpleDto
    {
        /// <summary>
        /// 编号
        /// </summary>
        public long NodeId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string? Logo { get; set; }
    }
}
