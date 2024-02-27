using Volo.Abp.Domain.Repositories;

namespace ContIn.Abp.Terminal.Domain.Users
{
    /// <summary>
    /// 消息仓储接口
    /// </summary>
    public interface IMessageRepository : IBasicRepository<Message, long>
    {
        /// <summary>
        /// 获取未读消息数量
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> GetUnReadCountAsync(long userId);
        /// <summary>
        /// 标记所有消息已读
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task MarkReadAsync(long userId);
        /// <summary>
        /// 获取最近未读消息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        Task<List<Message>> GetMsgRecentAsync(long userId, int limit = 3, bool includeDetails = false);
        /// <summary>
        /// 获取用户消息列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        Task<(int, List<Message>)> GetMessagesAsync(long userId, int page = 1, int limit = 10, bool includeDetails = false);
    }
}