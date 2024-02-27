namespace ContIn.Abp.Terminal.Application.Contracts
{
    /// <summary>
    /// 分页响应
    /// </summary>
    public class SitePagingResultDto<T>
    {
        /// <summary>
        /// 分页模型
        /// </summary>
        public SitePagingPage Page { get; set; }
        /// <summary>
        /// 列表数据
        /// </summary>
        public List<T> Results { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public SitePagingResultDto()
        { 
            Page = new SitePagingPage();
            Results = new List<T>();
        }
    }
    /// <summary>
    /// 分页模型
    /// </summary>
    public class SitePagingPage
    {
        /// <summary>
        /// 每页显示数
        /// </summary>
        public int Limit { get; set; } = 20;
        /// <summary>
        /// 当前页码
        /// </summary>
        public int Page { get; set; } = 1;
        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total { get; set; } = 0;
    }
}
