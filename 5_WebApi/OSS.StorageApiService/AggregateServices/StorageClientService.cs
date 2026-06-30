using Azure.Core;
using OSS.StorageApiService;
using Volo.Abp.DependencyInjection;

namespace OSS.StorageApiService;

/// <summary>
/// 对象存储实现
/// </summary>
//[Dependency(ServiceLifetime.Singleton)]
public partial class StorageClientService : IStorageClientService
{

    private IStorageClientService _client;
    public StorageClientService(IStorageClientService clientService)
    {
        _client = clientService;
    }

    /// <summary>
    /// 上传对象
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public ResultObject PutObjectAsync(ObjectCreateDto request)
    {
        return _client.PutObjectAsync(request);
    }

    /// <summary>
    /// 删除对象
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public ResultObject DeleteObject(StorageObjectNameDto request)
    {
       return _client.DeleteObject(request);   
    }

    /// <summary>
    /// 文件下载
    /// </summary>
    /// <param name="request"></param>
    /// <param name="downloadFilename"></param>
    public void ResumableGetObject(StorageObjectNameDto request, string downloadFilename)
    {
       _client.ResumableGetObject(request, downloadFilename);
    }

    /// <summary>
    /// 获取预签名
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<ResultObject> GetPreSignerUrlAsync(ObjectPreSignerRequest param)
    {
        return await _client.GetPreSignerUrlAsync(param);
    }

    /// <summary>
    /// 对象列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public ResultObject ListObjects(ObjectListRequest request)
    {
        return _client.ListObjects(request);
    }


    public Task<ResultObject> GetBucketListAsync()
    {
        return _client.GetBucketListAsync();
    }

    public void Dispose()
    {
       _client?.Dispose();
    }

}
