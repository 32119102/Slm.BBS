namespace ContIn.Abp.Terminal.Application.Contracts.Articles
{
    /// <summary>
    /// 修改文章标签
    /// </summary>
    public class UpdateArticleTagDto
    {
        /// <summary>
        /// 标签编号，逗号分隔
        /// </summary>
        public string? TagIds { get; set; }
    }
}
