using Volo.Abp.Domain.Entities;

namespace ContIn.Abp.Terminal.Domain.Sys
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class SysConfig : Entity<long>
    {
        /// <summary>
        /// 配置键
        /// </summary>
        public string? Key { get; set; }
        /// <summary>
        /// 配置值
        /// </summary>
        public string? Value { get; set; }
        /// <summary>
        /// 配置名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 配置描述
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTime { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public long UpdateTime { get; set; }
    }
}
