using Volo.Abp; 

namespace OSS.StorageApiService;

/// <summary>
/// 
/// </summary>
[RemoteService(IsEnabled = true, IsMetadataEnabled = true)]
public interface IProductImageStorageService 
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
