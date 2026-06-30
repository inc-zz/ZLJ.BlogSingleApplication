namespace OSS.StorageApiService
{
    /// <summary>
    /// minio配置
    /// </summary>
    public class MinioStorageConfig : BaseStorageModel
    {
        /// <summary>
        /// minio api地址
        /// </summary>
        public string? ApiAddress { get; set; }
        /// <summary>
        /// 访问令牌
        /// </summary>
        public string? Token { get; set; }

    }
}
