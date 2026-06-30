using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Configuration;

namespace OSS.StorageApiService;

public class ApiServiceDbContextFactory : IDesignTimeDbContextFactory<ApiServiceDbContext>
{
    public ApiServiceDbContext CreateDbContext(string[] args)
    {

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<ApiServiceDbContext>()
            .UseMySQL(configuration.GetConnectionString("Default"));

        return new ApiServiceDbContext(builder.Options);
    }

    /// <summary>
    /// º”‘ÿ≈‰÷√
    /// </summary>
    /// <returns></returns>
    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
