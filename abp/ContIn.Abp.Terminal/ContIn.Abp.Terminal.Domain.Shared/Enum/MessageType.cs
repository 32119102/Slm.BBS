namespace ContIn.Abp.Terminal.Domain.Shared
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageType
    {
        // 收到话题评论
        TopicComment = 0,
        // 收到他人回复
        CommentReply = 1,
        // 收到点赞
        TopicLike = 2,
        // 话题被收藏
        TopicFavorite = 3,
        // 话题被设为推荐
        TopicRecommend = 4,
        // 话题被删除
        TopicDelete = 5,
        // 收到文章评论
        ArticleComment = 6,
    }
}
