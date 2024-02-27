using ContIn.Abp.Terminal.Application.Contracts;
using ContIn.Abp.Terminal.Application.Contracts.Messages;
using ContIn.Abp.Terminal.Domain.Shared;
using ContIn.Abp.Terminal.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace ContIn.Abp.Terminal.Application.Messages
{
    /// <summary>
    /// 消息
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = SwaggerGroupName.User)]
    public class MessageAppService : ApplicationService, IMessageAppService
    {
        private readonly IMessageRepository _repository;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public MessageAppService(IMessageRepository repository)
        { 
            _repository = repository;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="fromId"></param>
        /// <param name="userId">消息接收人</param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="quoteContent"></param>
        /// <param name="type"></param>
        /// <param name="extraData"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task AddMessageAsync(long fromId, long userId, string title, string content, string quoteContent, MessageType type, string extraData)
        {
            if (userId == 0)
            {
                throw new CustomException(TerminalErrorCodes.CommonErrorCode, "消息接收人ID错误");
            }
            if (fromId == userId)
            {
                return;
            }
            if (title.IsNullOrWhiteSpace())
            {
                throw new CustomException(TerminalErrorCodes.CommonErrorCode, "消息标题不能为空");
            }
            var message = new Message()
            {
                FromId = fromId,
                UserId = userId,
                Title = title,
                Content = content,
                QuoteContent = quoteContent,
                Type = type,
                ExtraData = extraData,
                Status = MessageStatus.UnRead,
                CreateTime = Clock.Now.ToMillisecondsTimestamp()
            };
            await _repository.InsertAsync(message);
        }

        /// <summary>
        /// 获取用户消息列表
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<SitePagingResultDto<MessageDto>> GetMessagesAsync(int page = 1)
        {
            var userId = CurrentUser.GetUserId();
            int limit = 10;
            var messages = await _repository.GetMessagesAsync(userId, page, limit, true);
            // 设置已读
            await _repository.MarkReadAsync(userId);
            return new SitePagingResultDto<MessageDto>()
            {
                Page = new SitePagingPage()
                {
                    Limit = limit,
                    Page = page,
                    Total = messages.Item1
                },
                Results = ObjectMapper.Map<List<Message>, List<MessageDto>>(messages.Item2)
            };
        }

        /// <summary>
        /// 获取用户最近未读消息
        /// </summary>
        /// <returns></returns>
        public async Task<UserMessagesDto> GetMsgRecentAsync()
        {
            var userId = CurrentUser.GetUserId();
            var count = await _repository.GetUnReadCountAsync(userId);
            var messages = await _repository.GetMsgRecentAsync(userId);
            return new UserMessagesDto() { Count = count, Messages = ObjectMapper.Map<List<Message>, List<MessageDto>>(messages) };
        }
    }
}
