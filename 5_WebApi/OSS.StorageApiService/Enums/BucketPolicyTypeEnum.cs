namespace OSS.StorageApiService;


/// <summary>
/// 策略类型
/// </summary>
public enum BucketPolicyTypeEnum
{
    /// <summary>
    /// 列举对象
    /// </summary>
    LIST = 1,
    /// <summary>
    /// 写入对象
    /// </summary>
    WRITE,
    /// <summary>
    /// 读写对象
    /// </summary>
    READ_WRITE,
    /// <summary>
    /// 删除对象
    /// </summary>
    DELETE,
    /// <summary>
    /// 完全控制
    /// </summary>
    FULL_CONTROL,
    /// <summary>
    /// 操作桶
    /// </summary>
    BUCKET_CONTROL,
    /// <summary>
    /// ACL
    /// </summary>
    ACL_CONTROL

}
