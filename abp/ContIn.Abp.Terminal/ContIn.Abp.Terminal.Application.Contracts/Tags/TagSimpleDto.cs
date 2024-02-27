namespace ContIn.Abp.Terminal.Application.Contracts.Tags
{
    /// <summary>
    /// 标签
    /// </summary>
    public class TagSimpleDto
    {
        /// <summary>
        /// 标签编号
        /// </summary>
        public long TagId { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public string? TagName { get; set; }
    }
}
