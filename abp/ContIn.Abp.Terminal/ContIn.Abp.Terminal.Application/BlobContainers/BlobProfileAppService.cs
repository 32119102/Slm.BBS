using ContIn.Abp.Terminal.Application.Contracts.BlobContainers;
using ContIn.Abp.Terminal.Domain.BlobContainers;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;

namespace ContIn.Abp.Terminal.Application.BlobContainers
{
    /// <summary>
    /// 文件存储
    /// </summary>
    [Authorize]
    public class BlobProfileAppService : ApplicationService, IBlobProfileAppService
    {
        private readonly IBlobContainer<ProfileFileContainer> _container;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="container"></param>
        public BlobProfileAppService(IBlobContainer<ProfileFileContainer> container)
        { 
            _container = container;
        }
        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<BlobFileInputOutputDto> GetBlobProfileAsync(string? name)
        {
            return new BlobFileInputOutputDto()
            { 
                Name = name,
                Content = await _container.GetAllBytesAsync(name)
            };
        }
        /// <summary>
        /// 保存文件内容
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<string> SaveBlobProfileAsync(BlobFileInputOutputDto input)
        {
            await _container.SaveAsync(input.Name, input.Content, overrideExisting: true);
            return input.Name ?? string.Empty;
        }
    }
}
