using ContIn.Abp.Terminal.Application.Contracts.BlobContainers;
using ContIn.Abp.Terminal.Domain.BlobContainers;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;

namespace ContIn.Abp.Terminal.Application.BlobContainers
{
    /// <summary>
    /// 图片存储应用服务
    /// </summary>
    [Authorize]
    public class BlobPictureAppService : ApplicationService, IBlobPictureAppService
    {
        private readonly IBlobContainer<ProfilePictureContainer> _container;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="container"></param>
        public BlobPictureAppService(IBlobContainer<ProfilePictureContainer> container)
        {
            _container = container;
        }
        /// <summary>
        /// 读取BLOB
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<BlobPictureInputOutputDto> GetBlobPictureAsync(string? name)
        {
            var blob = await _container.GetAllBytesAsync(name!);
            return new BlobPictureInputOutputDto() { Name = name, Content = blob };
        }
        /// <summary>
        /// 保存BLOB
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<string> SaveBlobPictureAsync(BlobPictureInputOutputDto input)
        {
            await _container.SaveAsync(input.Name, input.Content, overrideExisting: true);
            return input.Name ?? string.Empty;
        }
    }
}
