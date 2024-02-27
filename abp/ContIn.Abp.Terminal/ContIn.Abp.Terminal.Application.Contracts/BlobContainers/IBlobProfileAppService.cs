using Volo.Abp.Application.Services;

namespace ContIn.Abp.Terminal.Application.Contracts.BlobContainers
{
    /// <summary>
    /// 文件存储接口
    /// </summary>
    public interface IBlobProfileAppService : IApplicationService
    {
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<string> SaveBlobProfileAsync(BlobFileInputOutputDto input);
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<BlobFileInputOutputDto> GetBlobProfileAsync(string? name);
    }
}
