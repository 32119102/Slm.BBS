namespace ContIn.Abp.Terminal.Application.Contracts.BlobContainers
{
    /// <summary>
    /// 图片上传响应，带缩略图
    /// </summary>
    public class BlobPictureWithPreviewDto
    {
        /// <summary>
        /// 原图地址
        /// </summary>
        public string? Url { get; set; }
        /// <summary>
        /// 缩略图预览地址
        /// </summary>
        public string? Preview { get; set; }
    }
}
