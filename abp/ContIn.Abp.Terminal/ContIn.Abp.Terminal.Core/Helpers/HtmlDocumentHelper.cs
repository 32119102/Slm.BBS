using HtmlAgilityPack;
using System;

namespace ContIn.Abp.Terminal.Core.Helpers
{
    public class HtmlDocumentHelper
    {
        private readonly HtmlDocument _htmlDocument;
        public HtmlDocumentHelper(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                throw new ArgumentNullException(html);
            }
            _htmlDocument = new HtmlDocument();
            _htmlDocument.LoadHtml(html);
        }

        /// <summary>
        /// 获取网站标题 /head/title
        /// </summary>
        /// <returns></returns>
        public string GetTitle()
        { 
            return _htmlDocument!.DocumentNode?.SelectSingleNode("//head/title")?.InnerHtml ?? string.Empty;
        }
        /// <summary>
        /// 获取网站描述 <meta name="description" content=""> 
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            return _htmlDocument!.DocumentNode?.SelectSingleNode("//meta[@name='description']")?.GetAttributeValue("content", string.Empty) ?? string.Empty;
        }
    }
}
