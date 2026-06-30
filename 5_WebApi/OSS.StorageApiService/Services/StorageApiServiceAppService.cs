 using Volo.Abp.Application.Services;

namespace OSS.StorageApiService;

/* Inherit your application services from this class. */
public abstract class StorageApiServiceAppService : ApplicationService
{
    protected StorageApiServiceAppService()
    {
        LocalizationResource = typeof(StorageApiServiceResource);
    }
}