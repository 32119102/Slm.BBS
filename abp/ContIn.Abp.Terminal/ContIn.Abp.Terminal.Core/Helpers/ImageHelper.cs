using SkiaSharp;
using System;
using System.IO;

namespace ContIn.Abp.Terminal.Core.Helpers
{
    /// <summary>
    /// 图像工具类
    /// </summary>
    public class ImageHelper
    {
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="sourceImage">原图字节流</param>
        /// <param name="destWidth">缩略图宽度</param>
        /// <param name="destHeight">缩略图高度</param>
        /// <returns>缩略图字节流</returns>
        public static byte[] CompressImgageByte(byte[] sourceImage, int destWidth = 350, int destHeight = 350)
        {
            using MemoryStream ms = new MemoryStream(sourceImage);
            using var source = SKBitmap.Decode(ms);
            // 原图宽高
            var sourceWidth = source.Width;
            var sourceHeight = source.Height;

            // 目标宽高都不小于原图宽高  或者  目标宽高都不大于0  或者  目标宽等于0但是目标高大于等于原图高  或者  目标高等于0但是目标宽大于等于原图宽
            if ((destWidth <= 0 && destHeight <= 0) 
                || (destWidth >= sourceWidth && destHeight >= sourceHeight)
                || (destWidth == 0 && destHeight >= sourceHeight)
                || (destHeight == 0 && destWidth >= sourceWidth))
            {
                destWidth = sourceWidth;
                destHeight = sourceHeight;
            }
            else
            {
                // 按比例缩放
                if (destWidth == 0)
                {
                    destWidth = Convert.ToInt32(sourceWidth * 0.1 / (sourceHeight * 0.1) * destHeight);
                }
                else if (destHeight == 0)
                {
                    destHeight = Convert.ToInt32(sourceHeight * 0.1 / (sourceWidth * 0.1) * destWidth);
                }
                else if ((destWidth * 0.1 / (sourceWidth * 0.1)) >= (destHeight * 0.1 / (sourceHeight * 0.1)))
                {
                    destWidth = Convert.ToInt32(sourceWidth * 0.1 / (sourceHeight * 0.1) * destHeight);
                }
                else if ((destWidth * 0.1 / (sourceWidth * 0.1)) < (destHeight * 0.1 / (sourceHeight * 0.1)))
                {
                    destHeight = Convert.ToInt32(sourceHeight * 0.1 / (sourceWidth * 0.1) * destWidth);
                }
                else
                {
                    destWidth = sourceWidth;
                    destHeight = sourceHeight;
                }
            }

            using var resized = source.Resize(new SKImageInfo(destWidth, destHeight), SKFilterQuality.High);
            using var image = SKImage.FromBitmap(resized);
            var data = image.Encode(SKEncodedImageFormat.Png, 75);

            using var outms = new MemoryStream();
            data.SaveTo(outms);
            return outms.ToArray();
        }
    }
}
