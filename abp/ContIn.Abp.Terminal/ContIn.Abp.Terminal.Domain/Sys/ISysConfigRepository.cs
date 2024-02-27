using Volo.Abp.Domain.Repositories;

namespace ContIn.Abp.Terminal.Domain.Sys
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public interface ISysConfigRepository : IBasicRepository<SysConfig, long>
    {
        /// <summary>
        /// 根据键获取信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<SysConfig?> GetByKeyAsync(string key);
    }
}
