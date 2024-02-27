using Volo.Abp.Domain.Repositories;

namespace ContIn.Abp.Terminal.Domain.Links
{
    /// <summary>
    /// 友情链接
    /// </summary>
    public interface IFriendlyLinkRepository : IBasicRepository<FriendlyLink, long>
    {
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="url"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        Task<List<FriendlyLink>> GetListAsync(int skipCount, int maxResultCount, string? url, string? title);
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="url"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        Task<int> GetCountAsync(string? url, string? title);
        /// <summary>
        /// 获取前10个链接
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<List<FriendlyLink>> GetTopLinksAsync(int limit = 10);
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<(int, List<FriendlyLink>)> GetPagingAsync(int page = 1, int limit = 20);
    }
}
