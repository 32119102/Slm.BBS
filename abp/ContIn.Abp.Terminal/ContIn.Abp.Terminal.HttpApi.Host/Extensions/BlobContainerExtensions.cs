using ContIn.Abp.Terminal.Domain.BlobContainers;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.BlobStoring.Minio;

namespace ContIn.Abp.Terminal.HttpApi.Host
{
    /// <summary>
    /// BLOB存储
    /// </summary>
    public static class BlobContainerExtensions
    {
        public static void AddBlobContainers(this IServiceCollection services)
        {
            var configuration = services.GetConfiguration();
            services.Configure<AbpBlobStoringOptions>(options => 
            {
                // 注册图片容器
                options.Containers.Configure<ProfilePictureContainer>(container => 
                {
                    // 文件系统
                    // container.UseFileSystem();
                    container.UseMinIO(configuration);
                });
                // 注册文件容器
                options.Containers.Configure<ProfileFileContainer>(container =>
                {
                    // container.UseFileSystem();
                    container.UseMinIO(configuration);
                });
            });
        }
        /// <summary>
        /// 文件系统提供程序
        /// </summary>
        /// <param name="config"></param>
        public static void UseFileSystem(this BlobContainerConfiguration container)
        {
            container.UseFileSystem(fileSystem =>
            {
                var filestreampath = Directory.GetCurrentDirectory() + @"/files";
                if (!Directory.Exists(filestreampath))
                {
                    Directory.CreateDirectory(filestreampath);
                }
                fileSystem.BasePath = filestreampath;
                fileSystem.AppendContainerNameToBasePath = true;
            });
        }

        /// <summary>
        /// MinIO 存储提供程序
        /// </summary>
        /// <param name="container"></param>
        /// <param name="configuration"></param>
        public static void UseMinIO(this BlobContainerConfiguration container, IConfiguration configuration)
        {
            container.UseMinio(minio =>
            {
                minio.EndPoint = configuration.GetValue<string>("MinIO:EndPoint");
                minio.AccessKey = configuration.GetValue<string>("MinIO:AccessKey");
                minio.SecretKey = configuration.GetValue<string>("MinIO:SecretKey");
                minio.BucketName = configuration.GetValue<string>("MinIO:BucketName");
                minio.WithSSL = false;
                minio.CreateBucketIfNotExists = true;
            });
        }
    }
}
