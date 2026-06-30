 using Volo.Abp.Application.Services;

namespace OSS.StorageApiService;

/* Inherit your application services from this class. */
public abstract class ApiServiceAppService : ApplicationService
{
    protected ApiServiceAppService()
    {
        LocalizationResource = typeof(ApiServiceResource);
    }
}