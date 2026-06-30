using Confluent.Kafka;
using Mapster;
using Newtonsoft.Json;
using StackExchange.Redis; 

namespace OSS.StorageApiService;

public partial class StorageClientService
{ 

    /// <summary>
    /// 获取ACL权限
    /// </summary>
    /// <param name="param"></param>
    public async Task<ResultObject<List<BucketAclResponse>>> GetAclAuthAsync(BucketNameRequest param)
    {
        return await _client.GetAclAuthAsync(param);
    }

    /// <summary>
    /// 更新存储桶ACL权限
    /// </summary>
    public async Task<ResultObject> PutAclAuthAsync(BucketAclRequest param)
    {
        return await _client.PutAclAuthAsync(param);
    }

    /// <summary>
    /// 删除ACL授权
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<ResultObject> DeleteBucketAclAsync(BucketNameRequest param)
    {
        return await _client.DeleteBucketAclAsync(param);
    }

}
