using System.ComponentModel.DataAnnotations;

namespace ContIn.Abp.Terminal.Application.Contracts.Authorize
{
    /// <summary>
    /// 刷新JwtToken
    /// </summary>
    public class RefreshTokenInputDto
    {
        /// <summary>
        /// token
        /// </summary>
        [Required]
        public string? Token { get; set; }
    }
}
