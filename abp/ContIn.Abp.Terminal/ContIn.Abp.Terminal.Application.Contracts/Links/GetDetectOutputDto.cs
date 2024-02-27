namespace ContIn.Abp.Terminal.Application.Contracts.Links
{
    /// <summary>
    /// 采集网站名称和描述
    /// </summary>
    public class GetDetectOutputDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }
    }
}
