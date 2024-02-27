using System;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;

namespace ContIn.Abp.Terminal.Domain.Shared
{
    public static class CurrentUserExtensions
    {
        /// <summary>
        /// 获取当前登录用户编号，如果没有返回0
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static long GetUserId(this ICurrentUser user)
        {
            return (user?.FindClaim(AbpClaimTypes.UserId)?.Value ?? "0").To<long>();
        }
    }
}
