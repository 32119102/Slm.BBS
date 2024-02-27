using ContIn.Abp.Terminal.Domain.Shared.Enum;
using Volo.Abp.Domain.Repositories;

namespace ContIn.Abp.Terminal.Domain.Comments
{
    /// <summary>
    /// 评论
    /// </summary>
    public interface ICommentRepository : IBasicRepository<Comment, long>
    {
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="id"></param>
        /// <param name="userIds"></param>
        /// <param name="entityType"></param>
        /// <param name="entityId"></param>
        /// <param name="status"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        Task<List<Comment>> GetListAsync(int skipCount, int maxResultCount, long id, long[]? userIds, string? entityType, long entityId, StatusEnum? status, bool includeDetails = false);
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userIds"></param>
        /// <param name="entityType"></param>
        /// <param name="entityId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<int> GetCountAsync(long id, long[]? userIds, string? entityType, long entityId, StatusEnum? status);
        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="entityId"></param>
        /// <param name="cursor"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        Task<List<Comment>> GetCommentsAsync(string entityType, long entityId, long cursor, int limit = 20, bool includeDetails = false);
        /// <summary>
        /// 获取二级回复列表
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="cursor"></param>
        /// <param name="limit"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        Task<List<Comment>> GetRepliesAsync(long commentId, long cursor, int limit = 20, bool includeDetails = false);

        /// <summary>
        /// 评论被回复（二级评论）
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        Task OnCommentAsync(long commentId);
        /// <summary>
        /// 点赞数+1
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        Task IncrLikeCountAsync(long commentId);
    }
}
