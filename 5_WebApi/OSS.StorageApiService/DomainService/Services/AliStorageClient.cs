using Aliyun.OSS;
using Aliyun.OSS.Common;
using Amazon.Runtime;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Minio;
using Volo.Abp.DependencyInjection;

namespace OSS.StorageApiService
{

    /// <summary>
    /// 
    /// </summary>
    //[Dependency(ServiceLifetime.Singleton)]
    public partial class AliStorageClient : IAliStorageClient
    {
        /// <summary>
        /// 获取配置
        /// </summary>
        protected AliyunStorageConfig config = null;

        /// <summary>
        /// 
        /// </summary>
        protected OssClient _client { get; set; }

        public AliStorageClient(IOptions<AliyunStorageConfig> options)
        {
            var config = options.Value;
            var clientConfig = new ClientConfiguration { Protocol = config.Secure == true ? Protocol.Https : Protocol.Http };
            clientConfig.IsCname = config.IsCname ?? false;
            _client = new OssClient(config.Endpoint, config.AccessKey, config.SecretKey, "", clientConfig);
        }



        public ResultObject PutObjectAsync(ObjectCreateDto request)
        {
            return new ResultObject
            {
                code = 200,
                message = "OK",
                success = true
            };
        }

        public ResultObject GetBuckets()
        {
            return new ResultObject
            {
                message = "bucket02"
            };
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<ResultObject> GetBucketListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResultObject> GetPreSignerUrlAsync(ObjectPreSignerRequest param)
        {
            throw new NotImplementedException();
        }

        public ResultObject DeleteObject(StorageObjectNameDto request)
        {
            throw new NotImplementedException();
        }

        public void ResumableGetObject(StorageObjectNameDto request, string downloadFilename)
        {
            throw new NotImplementedException();
        }

        public ResultObject ListObjects(ObjectListRequest request)
        {
            throw new NotImplementedException();
        }

  
    }
}
