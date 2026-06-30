using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Modularity; 

namespace OSS.StorageApiService;

/// <summary>
/// 对象存储服务
/// </summary>
public class StorageApiService : IStorageApiService
{
    public IStorageAggregateService _aggregateService { get; set; }
    private StorageTypeEnum _clientType;

	/// <summary>
	/// 
	/// </summary>
	public StorageApiService()
	{
		_clientType = StorageConfigs.GetStorageType();
	}


    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="bucketName"></param>
    /// <param name="objectName"></param>
    /// <param name="file"></param>
    /// <returns></returns>
    public ResultObject UploadObject(string bucketName, string objectName, IFormFile file)
    {
        var ms = file.OpenReadStream();
        var inputStream = file.OpenReadStream();
        var request = new ObjectCreateDto
        {
            BucketName = bucketName,
            ObjectName = objectName,
            InputStream = inputStream
        };
        //获取存储目录

        using var _client2 = _aggregateService.GetInstance(_clientType);
        var result = _client2.PutObjectAsync(request);
        return result;
    }

}
