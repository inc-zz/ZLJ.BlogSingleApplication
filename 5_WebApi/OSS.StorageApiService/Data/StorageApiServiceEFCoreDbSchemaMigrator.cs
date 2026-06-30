using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace OSS.StorageApiService;

public class StorageApiServiceEFCoreDbSchemaMigrator : ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public StorageApiServiceEFCoreDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the StorageApiServiceDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<StorageApiServiceDbContext>()
            .Database
            .MigrateAsync();
    }
}
