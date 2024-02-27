using System.ComponentModel.DataAnnotations;

namespace ContIn.Abp.Terminal.Application.Contracts.Users
{
    /// <summary>
    /// 新增用户点赞
    /// </summary>
    public class CreateUserLikeDto
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 实体类型
        /// </summary>
        [Required(ErrorMessage = "实体类型不能为空")]
        public string? EntityType { get; set; }
        /// <summary>
        /// 实体编号
        /// </summary>
        public long EntityId { get; set; }
    }
}
