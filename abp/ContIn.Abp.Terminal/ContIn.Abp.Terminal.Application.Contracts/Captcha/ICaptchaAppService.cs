using Volo.Abp.Application.Services;

namespace ContIn.Abp.Terminal.Application.Contracts.Captcha
{
    /// <summary>
    /// 验证码
    /// </summary>
    public interface ICaptchaAppService : IApplicationService
    {
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="captchaId"></param>
        /// <returns></returns>
        Task<CaptchaDto> GetRequestAsync(string? captchaId);

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="captchaId"></param>
        /// <param name="captchaCode"></param>
        /// <returns></returns>
        Task ValideCaptchaCodeAsync(string? captchaId, string? captchaCode);
    }
}
