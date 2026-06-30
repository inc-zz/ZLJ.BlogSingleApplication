using Volo.Abp;

namespace OSS.StorageApiService;

/// <summary>
/// 对象存储服务
/// </summary>
public interface IStorageApiService : IRemoteService
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bucketName"></param>
    /// <param name="objectName"></param>
    /// <param name="file"></param>
    /// <returns></returns>
    ResultObject UploadObject(string bucketName, string objectName, IFormFile file);


}
