
namespace OSS.StorageApiService
{
    /// <summary>
    /// 对象存储配置
    /// </summary>
    public class StorageConfigs
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static StorageTypeEnum GetStorageType()
        {
            StorageTypeEnum clientType = StorageTypeEnum.MinIO;
            var storageType =  ConfigHelper.Instance.GetSection("StorageConfig:StorageType").Value.ToString();
            switch (storageType)
            {
                case "Aws": 
                    clientType = StorageTypeEnum.Aws;
                    break;
                case "Aliyun": 
                    clientType = StorageTypeEnum.Aliyun;
                    break;
                case "MinIO": 
                    clientType = StorageTypeEnum.MinIO;
                    break;
            }
            return clientType;
        }

        /// <summary>
        /// 当前存储Bucket
        /// </summary>
        [ConfigurationKeyName("DefaultBucket")]
        public string? BucketName { get; set; }
        /// <summary>
        /// 配置项名称
        /// </summary>
        public string? StorageType { get; set; }


        
    }
}
