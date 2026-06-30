using Volo.Abp.Application.Services;

namespace OSS.StorageApiService;

/* Inherit your application services from this class. */
public abstract class ObjectStoreApiAppService : ApplicationService
{
    protected ObjectStoreApiAppService()
    {
        LocalizationResource = typeof(ObjectStoreApiResource);
    }
}