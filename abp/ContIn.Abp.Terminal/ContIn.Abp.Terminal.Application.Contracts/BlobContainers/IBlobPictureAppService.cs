using Volo.Abp.Application.Services;

namespace ContIn.Abp.Terminal.Application.Contracts.BlobContainers
{
    /// <summary>
    /// 图片存储
    /// </summary>
    public interface IBlobPictureAppService : IApplicationService
    {
        /// <summary>
        /// 保存图片BLOB
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<string> SaveBlobPictureAsync(BlobPictureInputOutputDto input);
        /// <summary>
        /// 读取BLOB
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<BlobPictureInputOutputDto> GetBlobPictureAsync(string? name);
    }
}
