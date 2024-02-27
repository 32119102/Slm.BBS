using ContIn.Abp.Terminal.Domain.Shared.Enum;
using Volo.Abp.Domain.Entities;

namespace ContIn.Abp.Terminal.Domain.Users
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public class OperateLog : Entity<long>
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public OpTypeEnum OpType { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public string? DataType { get; set; }
        /// <summary>
        /// 数据编号
        /// </summary>
        public long DataId { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        public string? IP { get; set; }
        /// <summary>
        /// UserAgent
        /// </summary>
        public string? UserAgent { get; set; }
        /// <summary>
        /// Referer
        /// </summary>
        public string? Referer { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTime { get; set; }
    }
}
