namespace OSS.StorageApiService;

public partial interface IStorageClientService: IDisposable
{

    /// <summary>
    /// 获取bucket
    /// </summary>
    /// <returns></returns>
    Task<ResultObject> GetBucketListAsync();
    /// <summary>
    /// 上传对象
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    ResultObject PutObjectAsync(ObjectCreateDto request);

    /// <summary>
    /// 获取预签名
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<ResultObject> GetPreSignerUrlAsync(ObjectPreSignerRequest param);

    /// <summary>
    /// 删除对象
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    ResultObject DeleteObject(StorageObjectNameDto request);
    /// <summary>
    /// 获取对象
    /// </summary>
    /// <param name="request"></param>
    /// <param name="downloadFilename"></param>
    void ResumableGetObject(StorageObjectNameDto request, string downloadFilename);
    /// <summary>
    /// 对象列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    ResultObject ListObjects(ObjectListRequest request);

}
