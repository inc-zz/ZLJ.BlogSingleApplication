using Volo.Abp.DependencyInjection;

namespace OSS.StorageApiService
{
    /// <summary>
    /// 
    /// </summary>
    [Dependency(ServiceLifetime.Singleton)]
    public class StorageAggregateService : IStorageAggregateService
    {
        private readonly IServiceProvider _serviceProvider;


        public StorageAggregateService(IServiceProvider serviceProvider)
        {
              _serviceProvider = serviceProvider;
        }


        public IStorageClientService GetInstance(StorageTypeEnum clientType)
        {
            object instance = null;
            switch (clientType)
            {
                case StorageTypeEnum.Aliyun:
                    instance = _serviceProvider.GetService(typeof(IAliStorageClient));
                    break;
                case StorageTypeEnum.MinIO:
                    instance = _serviceProvider.GetService(typeof(IMinIOStorageClient));
                    break;
            }
            return new StorageClientService(instance as IStorageClientService);

        }
 
    }
}
