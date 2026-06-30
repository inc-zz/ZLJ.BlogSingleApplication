using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace OSS.StorageApiService;

public class StorageApiServiceDbContext : AbpDbContext<StorageApiServiceDbContext>
{
    public StorageApiServiceDbContext(DbContextOptions<StorageApiServiceDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */
      
        /* Configure your own entities here */
    }
}
