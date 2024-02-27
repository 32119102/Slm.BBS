using Volo.Abp.Domain.Repositories;

namespace ContIn.Abp.Terminal.Domain.Topics
{
    /// <summary>
    /// 节点仓储接口
    /// </summary>
    public interface ITopicNodeRepository : IBasicRepository<TopicNode, long>
    {
        /// <summary>
        /// 根据名称获取信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<TopicNode?> FindByNameAsync(string name);
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<List<TopicNode>> GetListAsync(int skipCount, int maxResultCount, string? name);
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<int> GetCountAsync(string? name);
    }
}
