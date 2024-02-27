namespace System
{
    /// <summary>
    /// 时间扩展类
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 转换时间戳，秒
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long ToSecondsTimestamp(this DateTime time)
        {
            DateTimeOffset dt = new DateTimeOffset(time);
            return dt.ToUnixTimeSeconds();
        }
        /// <summary>
        /// 转换时间戳，毫秒
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long ToMillisecondsTimestamp(this DateTime time)
        {
            DateTimeOffset dt = new DateTimeOffset(time);
            return dt.ToUnixTimeMilliseconds();
        }
        /// <summary>
        /// 年月日，转换数字型
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long ToLongShortDate(this DateTime time)
        {
            return Convert.ToInt64(time.ToString("yyyyMMdd"));
        }
    }
}
