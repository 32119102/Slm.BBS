namespace ContIn.Abp.Terminal.Application.Contracts.BlobContainers
{
    /// <summary>
    /// 文件存储
    /// </summary>
    public class BlobFileInputOutputDto
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
