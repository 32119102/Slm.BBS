using AutoMapper;
using ContIn.Abp.Terminal.Application.Contracts.Articles;
using ContIn.Abp.Terminal.Application.Contracts.Comments;
using ContIn.Abp.Terminal.Application.Contracts.Feed;
using ContIn.Abp.Terminal.Application.Contracts.Follows;
using ContIn.Abp.Terminal.Application.Contracts.Links;
using ContIn.Abp.Terminal.Application.Contracts.Tags;
using ContIn.Abp.Terminal.Application.Contracts.Topics;
using ContIn.Abp.Terminal.Application.Contracts.Users;
using ContIn.Abp.Terminal.Domain.Articles;
using ContIn.Abp.Terminal.Domain.Comments;
using ContIn.Abp.Terminal.Domain.Feed;
using ContIn.Abp.Terminal.Domain.Links;
using ContIn.Abp.Terminal.Domain.Tags;
using ContIn.Abp.Terminal.Domain.Topics;
using ContIn.Abp.Terminal.Domain.Users;
using Newtonsoft.Json;

namespace ContIn.Abp.Terminal.Application
{
    /// <summary>
    /// 定义实体映射
    /// https://docs.automapper.org/en/stable/
    /// </summary>
    public class TerminalApplicationAutoMapperProfile : Profile
    {
        /// <summary>
        /// 设置映射关系
        /// </summary>
        public TerminalApplicationAutoMapperProfile()
        {
            // 标签
            CreateMap<Tag, TagDto>();
            CreateMap<Tag, TagSimpleDto>()
                .ForMember(dest => dest.TagId, m => m.MapFrom(src => src.Id))
                .ForMember(dest => dest.TagName, m => m.MapFrom(src => src.Name));
            CreateMap<TagDto, TagSimpleDto>()
                .ForMember(dest => dest.TagId, m => m.MapFrom(src => src.Id))
                .ForMember(dest => dest.TagName, m => m.MapFrom(src => src.Name));

            // 友情链接
            CreateMap<FriendlyLink, FriendlyLinkDto>();
            CreateMap<FriendlyLink, FriendlyLinkSimpleDto>()
                .ForMember(dest => dest.LinkId, m => m.MapFrom(src => src.Id));
            // 节点
            CreateMap<TopicNode, TopicNodeDto>();
            CreateMap<TopicNode, TopicNodeSimpleDto>()
                .ForMember(dest => dest.NodeId, m => m.MapFrom(src => src.Id));
            CreateMap<TopicNodeDto, TopicNodeSimpleDto>()
                .ForMember(dest => dest.NodeId, m => m.MapFrom(src => src.Id));

            // 话题
            CreateMap<Topic, TopicDto>()
                .ForMember(dest => dest.Summary, m => m.MapFrom(src => src.Content!.Length > 128 ? src.Content.Substring(0, 128) : src.Content))
                .ForMember(dest => dest.ImageList, m => m.MapFrom(src => JsonConvert.DeserializeObject<List<TopicImageDto>>(src.ImageList ?? "[]")))
                .ForMember(dest => dest.Tags, m => m.MapFrom(src => (src.TopicTags ?? new List<TopicTag>()).Select(x => x.Tag).ToList()));

            // 文章
            CreateMap<Article, ArticleDto>()
                .ForMember(dest => dest.Tags, m => m.MapFrom(src => (src.ArticleTags ?? new List<ArticleTag>()).Select(x => x.Tag).ToList()));
            CreateMap<Article, ArticleSimpleDto>()
                .ForMember(dest => dest.ArticleId, m => m.MapFrom(src => src.Id))
                .ForMember(dest => dest.Tags, m => m.MapFrom(src => (src.ArticleTags ?? new List<ArticleTag>()).Select(x => x.Tag).ToList()));
            CreateMap<Article, ArticleDetailDto>()
                .ForMember(dest => dest.ArticleId, m => m.MapFrom(src => src.Id))
                .ForMember(dest => dest.Tags, m => m.MapFrom(src => (src.ArticleTags ?? new List<ArticleTag>()).Select(x => x.Tag).ToList()));
            CreateMap<ArticleTag, ArticleTagDto>();
            // 评论
            CreateMap<Comment, CommentDto>();
            // 签到
            CreateMap<CheckIn, CheckInDto>()
                .ForMember(dest => dest.DayName, m => m.MapFrom(src => src.LatestDayName))
                .ForMember(dest => dest.CheckIn, m => m.MapFrom(src => DateTime.Now.ToLongShortDate() == src.LatestDayName));

            // Feed
            CreateMap<UserFeed, UserFeedDto>();
            // 粉丝关注
            CreateMap<UserFollow, UserFollowDto>();
        }
    }
}
