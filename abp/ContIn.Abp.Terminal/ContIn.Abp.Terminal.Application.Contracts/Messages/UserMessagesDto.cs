namespace ContIn.Abp.Terminal.Application.Contracts.Messages
{
    /// <summary>
    /// 用户消息
    /// </summary>
    public class UserMessagesDto
    {
        /// <summary>
        /// 未读消息数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 未读消息列表
        /// </summary>
        public List<MessageDto>? Messages { get; set; }
    }
}
