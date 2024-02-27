namespace ContIn.Abp.Terminal.Application.Contracts.Search
{
    /// <summary>
    /// 搜索
    /// </summary>
    public class SearchInputDto
    {
        /// <summary>
        /// 关键词
        /// </summary>
        public string? Keyword { get; set; }
        /// <summary>
        /// 节点编号
        /// 0：全部，-1：推荐
        /// </summary>
        public long NodeId { get; set; } = 0;
        /// <summary>
        /// 时间范围，0：时间不限，1：一天内，2：一周内，3：一月内，4：一年内
        /// </summary>
        public int TimeRange { get; set; } = 0;
        /// <summary>
        /// 页码
        /// </summary>
        public int Page { get; set; } = 1;
    }
}
