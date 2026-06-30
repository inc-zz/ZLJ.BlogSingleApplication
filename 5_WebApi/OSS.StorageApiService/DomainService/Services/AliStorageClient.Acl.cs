using Aliyun.OSS;
using Confluent.Kafka; 

namespace OSS.StorageApiService
{
    /// <summary>
    /// 
    /// </summary>
    public partial class AliStorageClient
    {

        public Task<ResultObject<List<BucketAclResponse>>> GetAclAuthAsync(BucketNameRequest param)
        {
            throw new NotImplementedException();
        }

        public Task<ResultObject> PutAclAuthAsync(BucketAclRequest param)
        {
            throw new NotImplementedException();
        }

        public Task<ResultObject> DeleteBucketAclAsync(BucketNameRequest param)
        {
            throw new NotImplementedException();
        }

    }
}
