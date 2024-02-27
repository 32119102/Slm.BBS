using Volo.Abp.Domain.Repositories;

namespace ContIn.Abp.Terminal.Domain.Tags
{
    /// <summary>
    /// 标签仓储接口
    /// </summary>
    public interface ITagRepository : IBasicRepository<Tag, long>
    {
        /// <summary>
        /// 根据标签名称获取信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<Tag?> FindByTagNameAsync(string name);
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<List<Tag>> GetListAsync(int skipCount, int maxResultCount, string? name);
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<int> GetCountAsync(string? name);
        /// <summary>
        /// 保存标签信息
        /// </summary>
        /// <param name="tagNames"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        Task<List<Tag>> GetOrCreateAsync(string[] tagNames, long time);
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<(int, List<Tag>)> GetPagingAsync(int page = 1, int limit = 100);
    }
}
