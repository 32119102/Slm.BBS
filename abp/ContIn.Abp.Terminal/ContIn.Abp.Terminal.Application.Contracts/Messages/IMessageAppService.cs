using ContIn.Abp.Terminal.Domain.Shared;
using Volo.Abp.Application.Services;

namespace ContIn.Abp.Terminal.Application.Contracts.Messages
{
    /// <summary>
    /// 消息应用服务接口
    /// </summary>
    public interface IMessageAppService : IApplicationService
    {
        /// <summary>
        /// 获取用户最近未读消息
        /// </summary>
        /// <returns></returns>
        Task<UserMessagesDto> GetMsgRecentAsync();

        /// <summary>
        /// 获取用户消息列表
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<SitePagingResultDto<MessageDto>> GetMessagesAsync(int page = 1);
        /// <summary>
        /// 新增消息
        /// </summary>
        /// <param name="fromId">消息发送人</param>
        /// <param name="userId">消息接收人</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="quoteContent">引用内容</param>
        /// <param name="type">类型</param>
        /// <param name="extraData">扩展属性</param>
        /// <returns></returns>
        Task AddMessageAsync(long fromId, long userId, string title, string content, string quoteContent, MessageType type, string extraData);
    }
}
