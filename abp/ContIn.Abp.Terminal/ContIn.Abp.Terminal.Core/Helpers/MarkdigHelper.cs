using Markdig;
using Markdig.Parsers;
using Markdig.Syntax;
using System;
using System.Linq;

namespace ContIn.Abp.Terminal.Core.Helpers
{
    /// <summary>
    /// Markdig is a fast, powerful, CommonMark compliant, extensible Markdown processor for .NET.
    /// https://github.com/xoofx/markdig
    /// </summary>
    public class MarkdigHelper
    {
        /// <summary>
        /// 从文章正文 <paramref name="content"/> 提取前 <paramref name="length"/> 字的摘要
        /// </summary>
        /// <param name="content"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetSummary(string? content, int length)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return string.Empty;
            }
            return Markdown.ToPlainText(content).Limit(length);
        }
    }
}
