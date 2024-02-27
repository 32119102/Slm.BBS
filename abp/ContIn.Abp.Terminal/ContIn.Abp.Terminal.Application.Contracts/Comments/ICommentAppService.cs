using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ContIn.Abp.Terminal.Application.Contracts.Comments
{
    /// <summary>
    /// 评论服务
    /// </summary>
    public interface ICommentAppService : IApplicationService
    {
        /// <summary>
        /// 列表查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<CommentDto>> PostPagedAsync(SearchCommentPagedDto input);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        Task DeleteAsync(long id);
        /// <summary>
        /// 获取评论
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="entityId">实体编号</param>
        /// <param name="cursor"></param>
        /// <returns></returns>
        Task<SitePagedResultDto<CommentSimpleDto>> GetCommentsAsync(string entityType, long entityId, long cursor = 0);
        /// <summary>
        /// 获取评论回复
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="cursor"></param>
        /// <returns></returns>
        Task<SitePagedResultDto<CommentSimpleDto>> GetRepliesAsync(long commentId, long cursor = 0);

        /// <summary>
        /// 新增评论
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<CommentSimpleDto> PostCreateAsync(CreateCommentDto input);
        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        Task PostLikedAsync(long commentId);
    }
}
