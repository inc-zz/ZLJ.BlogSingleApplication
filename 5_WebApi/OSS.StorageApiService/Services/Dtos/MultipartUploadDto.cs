namespace OSS.StorageApiService
{
    /// <summary>
    /// 分片上传
    /// </summary>
    public class MultipartUploadDto
    {
        /// <summary>
        /// 存储桶
        /// </summary>
        public string? BucketName { get; set; }
        /// <summary>
        /// 对象名称
        /// </summary>
        public string? ObjectName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? FilePath { get; set; }

    }
}
