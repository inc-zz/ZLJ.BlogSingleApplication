using Mapster;

namespace OSS.StorageApiService
{

    /// <summary>
    /// MinIO配置
    /// </summary>
    public class MinIOStorageModule : StorageApiServiceModule
    {
        /// <summary>
        /// 初始化MinIO配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MinioStorageConfig>(configuration.GetSection("StorageConfig:MinIOStorage"));

            TypeAdapterConfig<Minio.DataModel.Bucket, BucketInfoDto>
                .NewConfig()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.CreateDate, src => DateTime.Parse(src.CreationDate));

            services.AddTransient(typeof(IMinIOStorageClient), typeof(MinIOStorageClient));

        }
    }
}
