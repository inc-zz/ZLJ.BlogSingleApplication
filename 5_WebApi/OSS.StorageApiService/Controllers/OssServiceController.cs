
namespace OSS.StorageApiService
{
    /// <summary>
    /// 
    /// </summary>
    public class OssServiceController : IStorageApiService
    {
        public IStorageAggregateService _aggregateService { get; set; }
        public ApiServiceDbContext _dbContext { get; set; }
        private StorageTypeEnum _clientType;

        /// <summary>
        /// 
        /// </summary>
        public OssServiceController()
        {
            _clientType = StorageConfigs.GetStorageType();
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public ResultObject UploadObject(string bucketName, string objectName, IFormFile file)
        {
            var ms = file.OpenReadStream();
            var inputStream = file.OpenReadStream();
            var request = new ObjectCreateDto
            {
                BucketName = bucketName,
                ObjectName = objectName,
                InputStream = inputStream
            };

            //获取存储目录
            var list =  _dbContext.StorageConfig.FindAsync().GetAwaiter().GetResult();
            using var _client2 = _aggregateService.GetInstance(_clientType);
            var result = _client2.PutObjectAsync(request);
            return result;
        }
    }
}
