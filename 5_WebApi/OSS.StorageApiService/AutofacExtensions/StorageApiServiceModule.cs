namespace OSS.StorageApiService
{
    public class StorageApiServiceExtModule
    {

        public static void UseStorage(IServiceCollection services,IConfiguration configuration,Dictionary<StorageTypeEnum,Action<IServiceCollection,IConfiguration>> iocExtension)
        {
            services.AddTransient<IStorageAggregateService, StorageAggregateService>();
            foreach (KeyValuePair<StorageTypeEnum,Action<IServiceCollection,IConfiguration>> ioc in iocExtension)
            {
                try
                {
                    ioc.Value(services, configuration);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

    }
}
