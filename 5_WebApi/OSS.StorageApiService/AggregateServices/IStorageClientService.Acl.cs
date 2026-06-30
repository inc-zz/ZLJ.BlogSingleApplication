

namespace OSS.StorageApiService;

public partial interface IStorageClientService
{
    /// <summary>
    /// 获取ACL权限
    /// </summary>
    /// <param name="param"></param>
    Task<ResultObject<List<BucketAclResponse>>> GetAclAuthAsync(BucketNameRequest param);
    /// <summary>
    /// 更新存储桶ACL权限
    /// </summary>
    Task<ResultObject> PutAclAuthAsync(BucketAclRequest param);
    /// <summary>
    /// 删除ACL授权
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<ResultObject> DeleteBucketAclAsync(BucketNameRequest param);

}
