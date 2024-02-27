using ContIn.Abp.Terminal.Application.Contracts.Captcha;
using ContIn.Abp.Terminal.Core.Helpers;
using ContIn.Abp.Terminal.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Application.Services;
using Volo.Abp.Guids;

namespace ContIn.Abp.Terminal.Application.Captcha
{
    /// <summary>
    /// 验证码
    /// </summary>
    [AllowAnonymous]
    [ApiExplorerSettings(GroupName = SwaggerGroupName.Auth)]
    public class CaptchaAppService : ApplicationService, ICaptchaAppService
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IDistributedCache _cache;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="guidGenerator"></param>
        /// <param name="cache"></param>
        public CaptchaAppService(IGuidGenerator guidGenerator, IDistributedCache cache)
        {
            _guidGenerator = guidGenerator;
            _cache = cache;
        }
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="captchaId"></param>
        /// <returns></returns>
        [Route("/api/app/captcha/request")]
        public async Task<CaptchaDto> GetRequestAsync([FromQuery] string? captchaId)
        {
            if (captchaId.IsNullOrWhiteSpace())
            {
                captchaId = _guidGenerator.Create().ToString("n");
            }
            var code = CaptchaCodeHelper.GetRandomEnDigitalText(4);
            var image = await CaptchaCodeHelper.GenerateCaptchaImageAsync(code);

            await _cache.SetStringAsync(captchaId, code, new DistributedCacheEntryOptions() { AbsoluteExpiration = Clock.Now.AddMinutes(5) });

            return new CaptchaDto
            {
                CaptchaId = captchaId,
                CaptchaCodeImg = Convert.ToBase64String(image),
                ImgType = "image/png"
            };
        }

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="captchaId"></param>
        /// <param name="captchaCode"></param>
        /// <returns></returns>
        public async Task ValideCaptchaCodeAsync(string? captchaId, string? captchaCode)
        {
            if (captchaId.IsNullOrWhiteSpace())
            {
                throw new CustomException(TerminalErrorCodes.ArgumentIsEmpty, "验证码编号为空");
            }

            if (captchaCode.IsNullOrWhiteSpace())
            {
                throw new CustomException(TerminalErrorCodes.ArgumentIsEmpty, "验证码为空");
            }

            var c = await _cache.GetStringAsync(captchaId);
            if (c.IsNullOrWhiteSpace())
            {
                throw new CustomException(TerminalErrorCodes.CaptchaOutTime, "验证码已过期");
            }

            if (!c.Equals(captchaCode, StringComparison.OrdinalIgnoreCase))
            {
                throw new CustomException(TerminalErrorCodes.CaptchaNotEqual, "验证码错误");
            }
        }
    }
}
