using ContIn.Abp.Terminal.Application.Contracts.BlobContainers;
using ContIn.Abp.Terminal.Core.Helpers;
using ContIn.Abp.Terminal.Domain.Shared;
using ContIn.Abp.Terminal.Domain.Shared.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;

namespace ContIn.Abp.Terminal.HttpApi.Controllers
{
    /// <summary>
    /// 文件操作
    /// </summary>
    [Route("api/file")]
    [ApiExplorerSettings(GroupName = SwaggerGroupName.Common)]
    public class FileController : AbpController
    {
        private readonly IBlobPictureAppService _pictureService;
        private readonly IBlobProfileAppService _fileService;
        private readonly AppOptions _appOptions;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pictureAppService"></param>
        /// <param name="fileAppService"></param>
        /// <param name="appOptions"></param>
        public FileController(IBlobPictureAppService pictureAppService, IBlobProfileAppService fileAppService, IOptionsMonitor<AppOptions> appOptions)
        {
            _pictureService = pictureAppService;
            _fileService = fileAppService;
            _appOptions = appOptions.CurrentValue;
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, Route("blob-picture-save")]
        public async Task<string> SaveProPictureBlobAsync(IFormFile file)
        {
            // 文件名称
            var fileOriginName = file.FileName;
            var newFileName = GuidGenerator.Create() + "_" + fileOriginName;
            // 文件类型
            var contentType = file.ContentType;
            // 文件大小
            CheckRequestSizeLimit(file.Length);
            // 文件内容
            using var stream = file.OpenReadStream();
            var content = await stream.GetAllBytesAsync();

            await _pictureService.SaveBlobPictureAsync(new BlobPictureInputOutputDto()
            {
                Name = newFileName,
                Content = content
            });
            return _appOptions.Domain + "/api/file/blob-picture-visit?name=" + newFileName;
        }

        /// <summary>
        /// 上传图片，生成缩略图
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, Route("blob-picture-save-preview")]
        public async Task<BlobPictureWithPreviewDto> SaveBlogPictureWithPreviewAsync(IFormFile file)
        {
            // 文件名称
            var fileOriginName = file.FileName;
            var newFileName = GuidGenerator.Create() + "_" + fileOriginName;
            // 缩略图名称
            var newPreviewFileName = "preview_" + newFileName;
            // 文件类型
            var contentType = file.ContentType;
            // 文件大小
            CheckRequestSizeLimit(file.Length);
            // 文件内容
            using var stream = file.OpenReadStream();
            var content = await stream.GetAllBytesAsync();

            // 生成缩略图
            var previewContent = ImageHelper.CompressImgageByte(content);
            // 保存原图
            await _pictureService.SaveBlobPictureAsync(new BlobPictureInputOutputDto()
            {
                Name = newFileName,
                Content = content
            });
            // 保存缩略图
            await _pictureService.SaveBlobPictureAsync(new BlobPictureInputOutputDto()
            {
                Name = newPreviewFileName,
                Content = previewContent
            });
            return new BlobPictureWithPreviewDto()
            {
                Url = _appOptions.Domain + "/api/file/blob-picture-visit?name=" + newFileName,
                Preview = _appOptions.Domain + "/api/file/blob-picture-visit?name=" + newPreviewFileName
            };
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, Route("blob-file-save")]
        public async Task<string> SaveProfileBlobAsync(IFormFile file)
        {
            // 文件名称
            var fileOriginName = file.FileName;
            var newFileName = GuidGenerator.Create() + "_" + fileOriginName;
            // 文件类型
            var contentType = file.ContentType;
            // 文件大小
            CheckRequestSizeLimit(file.Length);
            // 文件内容
            using var stream = file.OpenReadStream();
            var content = await stream.GetAllBytesAsync();

            await _fileService.SaveBlobProfileAsync(new BlobFileInputOutputDto()
            {
                Name = newFileName,
                Content = content
            });
            return _appOptions.Domain + "/api/file/blob-picture-visit?name=" + newFileName;
        }

        /// <summary>
        /// 访问图片
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet, Route("blob-picture-visit")]
        public async Task<IActionResult> VisitProPictureAsync(string name)
        {
            var pictureDto = await _pictureService.GetBlobPictureAsync(name);
            // Convert.ToBase64String(pictureDto.Content);
            return File(pictureDto.Content!, "application/octet-stream", pictureDto.Name);
        }

        /// <summary>
        /// 访问文件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet, Route("blob-file-visit")]
        public async Task<IActionResult> VisitProfileAsync(string name)
        {
            var file = await _fileService.GetBlobProfileAsync(name);
            return File(file.Content!, "application/octet-stream", file.Name);
        }

        /// <summary>
        /// 检查文件大小限制
        /// 20M
        /// </summary>
        /// <param name="length"></param>
        /// <exception cref="CustomException"></exception>
        private void CheckRequestSizeLimit(long length)
        {
            if (length > 20 * 1024 * 1024)
            {
                throw new CustomException(TerminalErrorCodes.RequestSizeLimit, "上传文件大小超过限制");
            }
        }
    }
}
