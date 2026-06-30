namespace OSS.StorageApiService
{
    /// <summary>
    /// 存储配置
    /// </summary>
    public class BaseStorageModel
    {
        /// <summary>
        /// 接口地址
        /// </summary>
        public string? Endpoint { get; set; }
        /// <summary>
        /// 公钥
        /// </summary>
        public string? AccessKey { get; set; }
        /// <summary>
        /// 私钥
        /// </summary>
        public string? SecretKey { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        public string? Region { get; set; }
        /// <summary>
        /// 区域码
        /// </summary>
        public bool? Secure { get; set; }

    }
}
