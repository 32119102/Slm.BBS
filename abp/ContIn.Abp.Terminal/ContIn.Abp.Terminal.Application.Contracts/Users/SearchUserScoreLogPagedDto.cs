using Volo.Abp.Application.Dtos;

namespace ContIn.Abp.Terminal.Application.Contracts.Users
{
    /// <summary>
    /// 用户积分记录
    /// </summary>
    public class SearchUserScoreLogPagedDto : PagedResultRequestDto
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }
    }
}
