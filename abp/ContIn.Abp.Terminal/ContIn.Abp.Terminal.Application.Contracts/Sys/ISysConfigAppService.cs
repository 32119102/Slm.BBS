using Volo.Abp.Application.Services;

namespace ContIn.Abp.Terminal.Application.Contracts.Sys
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public interface ISysConfigAppService : IApplicationService
    {
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        Task<SysConfigDto> GetAllAsync();
        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task PostSaveAsync(SysConfigDto input);
    }
}
