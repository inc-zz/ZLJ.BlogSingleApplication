using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace OSS.StorageApiService;

public class ApiServiceEFCoreDbSchemaMigrator : ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public ApiServiceEFCoreDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the ApiServiceDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<ApiServiceDbContext>()
            .Database
            .MigrateAsync();
    }
}
