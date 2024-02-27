using ContIn.Abp.Terminal.Application.Contracts.Tags;
using ContIn.Abp.Terminal.Application.Contracts.Users;
using ContIn.Abp.Terminal.Domain.Shared.Enum;

namespace ContIn.Abp.Terminal.Application.Contracts.Topics
{
    /// <summary>
    /// 话题
    /// </summary>
    public class TopicSimpleDto
    {
        /// <summary>
        /// 话题编号
        /// </summary>
        public long TopicId { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public long Type { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public StatusEnum Status { get; set; }

        /// <summary>
        /// 图片
        /// [{"preview":"","url":""}]
        /// </summary>
        public List<TopicImageDto>? ImageList { get; set; }
        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool Recommend { get; set; }
        /// <summary>
        /// 推荐时间
        /// </summary>
        public long RecommendTime { get; set; }
        /// <summary>
        /// 查看数量
        /// </summary>
        public int ViewCount { get; set; }
        /// <summary>
        /// 跟帖数量
        /// </summary>
        public int CommentCount { get; set; }
        /// <summary>
        /// 点赞数量
        /// </summary>
        public int LikeCount { get; set; }
        /// <summary>
        /// 最后回复时间
        /// </summary>
        public long LastCommentTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTime { get; set; }
        /// <summary>
        /// 摘要
        /// 内容前128位
        /// </summary>
        public string? Summary { get; set; }
        /// <summary>
        /// 节点编号
        /// </summary>
        public long NodeId { get; set; }
        /// <summary>
        /// 节点信息
        /// </summary>
        public TopicNodeSimpleDto? Node { get; set; }
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserSimpleDto? User { get; set; }
        /// <summary>
        /// 标签信息
        /// </summary>
        public List<TagSimpleDto>? Tags { get; set; }
        /// <summary>
        /// 是否点赞
        /// </summary>
        public bool Liked { get; set; }

        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool Sticky { get; set; }
        /// <summary>
        /// 置顶时间
        /// </summary>
        public long StickyTime { get; set; }
    }
}
