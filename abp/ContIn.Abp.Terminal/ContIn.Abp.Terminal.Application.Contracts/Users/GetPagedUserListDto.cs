using Volo.Abp.Application.Dtos;

namespace ContIn.Abp.Terminal.Application.Contracts.Users
{
    /// <summary>
    /// 用户
    /// </summary>
    public class GetPagedUserListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string? NickName { get; set; }
    }
}
