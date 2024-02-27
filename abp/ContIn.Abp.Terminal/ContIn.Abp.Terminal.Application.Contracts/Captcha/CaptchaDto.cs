namespace ContIn.Abp.Terminal.Application.Contracts.Captcha
{
    /// <summary>
    /// 验证码
    /// </summary>
    public class CaptchaDto
    {
        /// <summary>
        /// 验证码编号
        /// </summary>
        public string? CaptchaId { get; set; }
        /// <summary>
        /// 验证码图片，base64格式
        /// </summary>
        public string? CaptchaCodeImg { get; set; }
        /// <summary>
        /// 图片类型
        /// </summary>
        public string? ImgType { get; set; }
    }
}
