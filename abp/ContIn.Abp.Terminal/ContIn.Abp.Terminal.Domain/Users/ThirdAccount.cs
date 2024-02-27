using ContIn.Abp.Terminal.Domain.Shared.Enum;
using Volo.Abp.Domain.Entities;

namespace ContIn.Abp.Terminal.Domain.Users
{
    /// <summary>
    /// 第三方账号
    /// </summary>
    public class ThirdAccount : Entity<long>
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public virtual long UserId { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public virtual string? Avatar { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public virtual string? Nickname { get; set; }
        /// <summary>
        /// 第三方类型
        /// </summary>
        public virtual ThirdAccountTypeEnum ThirdType { get; set; }
        /// <summary>
        /// 第三方唯一标识
        /// </summary>
        public virtual string? ThirdId { get; set; }
        /// <summary>
        /// 扩展数据
        /// </summary>
        public virtual string? ExtraData { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual long CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual long UpdateTime { get; set; }
    }
}
