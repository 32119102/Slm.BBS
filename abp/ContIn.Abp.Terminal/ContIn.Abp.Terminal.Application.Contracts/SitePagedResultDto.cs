namespace ContIn.Abp.Terminal.Application.Contracts
{
    /// <summary>
    /// 分页响应数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SitePagedResultDto<T>
    {
        /// <summary>
        /// 游标，上一条数据编号
        /// </summary>
        public long Cursor { get; set; }
        /// <summary>
        /// 是否还有更多
        /// </summary>
        public bool HasMore { get; set; }
        /// <summary>
        /// 列表数据
        /// </summary>
        public List<T>? Results { get; set; }
    }
}
