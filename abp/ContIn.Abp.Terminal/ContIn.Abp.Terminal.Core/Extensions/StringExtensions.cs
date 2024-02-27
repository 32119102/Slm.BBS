using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace System
{
    public static class StringExtensions
    {
        /// <summary>
        /// 字符串首字母小写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string FirstCharToLower(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return s;
            return s.First().ToString().ToLower() + s.Substring(1);
        }

        /// <summary>
        /// 截取字符串前 <paramref name="length"/> 长度字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Limit(this string str, int length)
        {
            if (str.Length <= length)
            {
                return str;
            }
            return str[..length];
        }
    }
}
