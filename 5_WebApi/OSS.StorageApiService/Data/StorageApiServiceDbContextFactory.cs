using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OSS.StorageApiService;

public class StorageApiServiceDbContextFactory : IDesignTimeDbContextFactory<StorageApiServiceDbContext>
{
    public StorageApiServiceDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<StorageApiServiceDbContext>()
            .UseMySQL(configuration.GetConnectionString("Default"));

        return new StorageApiServiceDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
