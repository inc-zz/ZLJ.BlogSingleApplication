using ZLJ.AbpFramework.Helpers; 

namespace OSS.StorageApiService
{
    public class StorageConfigService
    {

        private static StorageConfigService _appSetting = null;

        private static MinioStorageConfig _minioConfig = null;
        public static object _object = new object();

        /// <summary>
        /// 存储类别（MinIO/AWS/Aliyun）
        /// </summary>
        public static string StorageName { get; set; }

        private StorageConfigService()
        {

        }

        public static StorageConfigService Instance
        {
            get
            {
                if (_appSetting == null)
                {
                    lock (_object)
                    {
                        _appSetting = new StorageConfigService();
                        Thread.Sleep(10);
                    }
                }
                return _appSetting;
            }
        }

        /// <summary>
        /// Minio配置
        /// </summary>
        public MinioStorageConfig minioConfig
        {
            get
            {
                if (_minioConfig == null)
                {
                    try
                    {
                        _minioConfig = ConfigHelper.Instance.GetValue<MinioStorageConfig>("StorageConfig:MinIOStorage");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"读取MInIO存储配置失败:{e.Message}");
                    }
                }
                return _minioConfig;
            }
        }

    }
}
