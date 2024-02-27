using AutoMapper;
using ContIn.Abp.Terminal.Application.Contracts.Favorites;
using ContIn.Abp.Terminal.Application.Contracts.Users;
using ContIn.Abp.Terminal.Domain.Users;

namespace ContIn.Abp.Terminal.Application
{
    /// <summary>
    /// 用户实体映射
    /// </summary>
    public class UserAutoMapperProfile : Profile
    {
        /// <summary>
        /// 设置映射
        /// </summary>
        public UserAutoMapperProfile()
        {
            // 用户
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Roles, m => m.MapFrom(src => (src.Roles ?? string.Empty).Split(",", StringSplitOptions.RemoveEmptyEntries).ToList()))
                .ForMember(dest => dest.Description, m => m.MapFrom(src => src.Description.IsNullOrWhiteSpace() ? "这家伙很懒，什么都没留下" : src.Description));

            CreateMap<User, UserSimpleDto>()
                .ForMember(dest => dest.SmallAvatar, m => m.MapFrom(src => src.Avatar))
                .ForMember(dest => dest.Description, m => m.MapFrom(src => src.Description.IsNullOrWhiteSpace() ? "这家伙很懒，什么都没留下" : src.Description));

            CreateMap<UserDto, UserSimpleDto>()
               .ForMember(dest => dest.SmallAvatar, m => m.MapFrom(src => src.Avatar))
               .ForMember(dest => dest.Description, m => m.MapFrom(src => src.Description.IsNullOrWhiteSpace() ? "这家伙很懒，什么都没留下" : src.Description));

            CreateMap<UserDto, UserProfileDto>()
                .ForMember(dest => dest.SmallAvatar, m => m.MapFrom(src => src.Avatar))
                .ForMember(dest => dest.Description, m => m.MapFrom(src => src.Description.IsNullOrWhiteSpace() ? "这家伙很懒，什么都没留下" : src.Description))
                .ForMember(dest => dest.PasswordSet, m => m.MapFrom(src => src.Password.IsNullOrWhiteSpace() ? false : src.Password!.Length > 0));

            // 用户积分记录
            CreateMap<UserScoreLog, UserScoreLogDto>();
            // 点赞
            CreateMap<UserLike, UserLikeDto>();
            // 收藏
            CreateMap<UserFavorite, UserFavoriteDto>();
            CreateMap<UserFavorite, UserFavoriteSimpleDto>()
                .ForMember(dest => dest.FavoriteId, m => m.MapFrom(src => src.Id));
        }
    }
}
