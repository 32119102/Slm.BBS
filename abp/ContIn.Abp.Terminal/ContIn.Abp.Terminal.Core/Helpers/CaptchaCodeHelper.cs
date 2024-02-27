using SkiaSharp;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ContIn.Abp.Terminal.Core.Helpers
{
    /// <summary>
    /// 验证码工具类
    /// </summary>
    public class CaptchaCodeHelper
    {
        private static readonly SKColor[] _colors = { SKColors.Black, SKColors.Red, SKColors.DarkBlue, SKColors.Green, SKColors.Orange, SKColors.Brown, SKColors.DarkCyan, SKColors.Purple };
        private static readonly char[] _chars = { '2', '3', '4', '5', '6', '7', '8', '9',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'j', 'k', 'm', 'n', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };
        private static readonly Random _random = new Random();
        /// <summary>
        /// 生成随机英文字母/数字组合字符串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomEnDigitalText(int length)
        {
            StringBuilder sb = new StringBuilder();
            if (length > 0)
            {
                do
                {
                    if (_random.Next(0, 2) > 0)
                    {
                        sb.Append(_random.Next(2, 10));
                    }
                    else
                    {
                        sb.Append(_chars[_random.Next(0, _chars.Length)]);
                    }
                }
                while (--length > 0);
            }
            return sb.ToString();
        }
        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static async Task<byte[]> GenerateCaptchaImageAsync(int length)
        {
            return await GenerateCaptchaImageAsync(GetRandomEnDigitalText(length));
        }
        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="captchaCode"></param>
        /// <param name="imageWigth"></param>
        /// <param name="imageHeight"></param>
        /// <returns></returns>
        public static async Task<byte[]> GenerateCaptchaImageAsync(string captchaCode, int imageWidth = 90, int imageHeight = 35, int lineNum = 2, int lineWeight = 1)
        {
            // 创建bitmap位图
            using var image = new SKBitmap(imageWidth, imageHeight, SKColorType.Bgra8888, SKAlphaType.Premul);
            // 创建画笔
            using var canvas = new SKCanvas(image);
            // 填充背景色为白色
            canvas.Clear(SKColor.Empty);
            // 添加噪点
            for (int i = 0; i < imageWidth * 2; i++)
            {
                image.SetPixel(_random.Next(imageWidth), _random.Next(imageHeight), _colors[_random.Next(_colors.Length)]);
            }
            // 画干扰线
            using var disturbStyle = new SKPaint();
            for (int i = 0; i < lineNum; i++)
            {
                disturbStyle.Color = _colors[_random.Next(_colors.Length)];
                disturbStyle.StrokeWidth = lineWeight;
                canvas.DrawLine(_random.Next(imageWidth), _random.Next(imageHeight), _random.Next(imageWidth), _random.Next(imageHeight), disturbStyle);
            }
            // 画验证码
            using var drawStyle = new SKPaint();
            drawStyle.IsAntialias = true;
            drawStyle.TextSize = 24;
            for (int i = 0; i < captchaCode.Length; i++)
            {
                // 转动角度
                //float angle = _random.Next(-20, 20);
                //canvas.Translate(12, 12);
                //float px = i * 24;
                //float py = imageHeight / 2;
                //canvas.RotateDegrees(angle, px, py);
                // 随机一种文字颜色
                drawStyle.Color = _colors[_random.Next(_colors.Length)];
                var y = (i + 1) % 2 == 0 ? 20 : 30;
                canvas.DrawText(captchaCode.Substring(i, 1), 15 + (i * 15), y, drawStyle);
                // 恢复角度
                //canvas.RotateDegrees(-angle, px, py);
                //canvas.Translate(-12, -12);
            }

            using var img = SKImage.FromBitmap(image);
            using var p = img.Encode(SKEncodedImageFormat.Png, 100);
            return await Task.FromResult(p.ToArray());
        }
    }
}
