using ContIn.Abp.Terminal.Application.Contracts.Feed;
using ContIn.Abp.Terminal.Application.Contracts.Follows;
using ContIn.Abp.Terminal.Domain.Feed;
using ContIn.Abp.Terminal.Domain.Shared;
using ContIn.Abp.Terminal.Domain.Shared.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace ContIn.Abp.Terminal.Application.Feed
{
    /// <summary>
    /// Feed流
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = SwaggerGroupName.User)]
    public class UserFeedAppService : ApplicationService, IUserFeedAppService
    {
        private readonly IUserFeedRepository _repository;
        private readonly IFollowAppService _followAppService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="followAppService"></param>
        public UserFeedAppService(IUserFeedRepository repository, IFollowAppService followAppService)
        { 
            _repository = repository;
            _followAppService = followAppService;
        }

        /// <summary>
        /// 新增Feed流
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="dataId"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task CreateByUserAsync(long userId, long dataId, EntityTypeEnum dataType)
        {
            // 获取 userId 的粉丝
            int limit = 50;
            long cursor = 0;
            var fans = await _followAppService.GetFansAsync(userId, cursor, limit);
            do
            {
                if (fans.Results != null)
                {
                    foreach (var f in fans.Results)
                    {
                        await _repository.InsertAsync(new UserFeed()
                        {
                            AuthorId = userId,
                            CreateTime = Clock.Now.ToMillisecondsTimestamp(),
                            DataId = dataId,
                            DataType = dataType.ToString().ToLower(),
                            UserId = f.Id
                        });
                    }
                }
                cursor = fans.Cursor;
                fans = await _followAppService.GetFansAsync(userId, cursor, limit);
            } while (fans.HasMore);
        }

        /// <summary>
        /// 根据实体删除
        /// </summary>
        /// <param name="dataId"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task DeleteByDataIdAsync(long dataId, EntityTypeEnum dataType)
        {
            await _repository.DeleteByDataIdAsync(dataId, dataType);
        }

        /// <summary>
        /// 根据用户删除
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="authorId"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task DeleteByUserAsync(long userId, long authorId)
        {
            await _repository.DeleteByUserAsync(userId, authorId);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cursor"></param>
        /// <param name="dataType"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<UserFeedDto>> GetUserFeedsAsync(long userId, long cursor, EntityTypeEnum dataType, int limit)
        {
            var feeds = await _repository.GetUserFeedsAsync(userId, cursor, dataType, limit);
            return ObjectMapper.Map<List<UserFeed>, List<UserFeedDto>>(feeds ?? new List<UserFeed>());
        }
    }
}
