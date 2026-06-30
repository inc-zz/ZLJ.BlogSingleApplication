 
namespace OSS.StorageApiService;


/// <summary>
/// 
/// </summary>
public class ProductImageStorageService: IProductImageStorageService
{

    public ProductImageStorageService()
    {

    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="bucketName"></param>
    /// <param name="objectName"></param>
    /// <param name="file"></param>
    /// <returns></returns>
    public ResultObject UploadObject(string bucketName, string objectName, IFormFile file)
    {
        return new ResultObject();

    }


}
