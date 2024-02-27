namespace ContIn.Abp.Terminal.Application.Contracts.Articles
{
    /// <summary>
    /// 文章
    /// </summary>
    public class ArticleDetailDto : ArticleSimpleDto
    {
        /// <summary>
        /// 文章详情
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// 文章html详情
        /// </summary>
        public string? HtmlContent { get; set; }
    }
}
