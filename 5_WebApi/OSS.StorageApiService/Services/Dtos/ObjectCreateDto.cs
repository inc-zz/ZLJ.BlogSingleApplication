 

namespace OSS.StorageApiService
{
    /// <summary>
    /// 对象上传
    /// </summary>
    public class ObjectCreateDto : BucketNameRequest
    {
        /// <summary>
        /// 对象名 附带目录的创建方式：directory1/directory1/filename.extension
        /// </summary>
        public string ObjectName { get; set; }
        /// <summary>
        /// 对象文件流
        /// </summary>
        public Stream InputStream { get; set; }
        /// <summary>
        /// 路径，directory1/directory1/filename.extension 
        /// </summary>
        public string FilePath { get; set; }

    }
}
