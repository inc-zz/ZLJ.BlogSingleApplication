
namespace OSS.StorageApiService;


/// <summary>
/// 
/// </summary>
public interface IStorageAggregateService
{
    /// <summary>
    /// 获取存储实例
    /// </summary>
    /// <param name="clientType"></param>
    /// <returns></returns>
    IStorageClientService GetInstance(StorageTypeEnum clientType);
}
