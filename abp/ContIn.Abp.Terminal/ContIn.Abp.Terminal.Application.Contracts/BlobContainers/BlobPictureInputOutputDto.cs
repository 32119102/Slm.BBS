namespace ContIn.Abp.Terminal.Application.Contracts.BlobContainers
{
    /// <summary>
    /// 图片存储
    /// </summary>
    public class BlobPictureInputOutputDto
    {
        /// <summary>
        /// 文件内容
        /// </summary>
        public byte[]? Content { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string? Name { get; set; }
    }
}
