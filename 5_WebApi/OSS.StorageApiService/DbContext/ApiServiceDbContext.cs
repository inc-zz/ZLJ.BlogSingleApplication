using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using ZLJ.Storage.ApiService.Entities;

namespace OSS.StorageApiService;

/// <summary>
/// 
/// </summary>
public class ApiServiceDbContext : AbpDbContext<ApiServiceDbContext>
{
    public ApiServiceDbContext(DbContextOptions<ApiServiceDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// 订单评论图片表
    /// </summary>
    public DbSet<OssOrderCommentImage> OssOrderCommentImageDb { get; set; }
    /// <summary>
    /// 商品图片表
    /// </summary>
    public DbSet<OssProductImage> OssProductImage { get; set; }
    /// <summary>
    /// 商品商品表
    /// </summary>
    public DbSet<OssProductVideo> OssProductVideo { get; set; }
    /// <summary>
    /// Blog图片表
    /// </summary>
    public DbSet<OssShopBlogImage> OssShopBlogImage { get; set; }
    /// <summary>
    /// Blog视频表
    /// </summary>
    public DbSet<OssShopBlogVideo> OssShopBlogVideo { get; set; }
    /// <summary>
    /// 用户图片表
    /// </summary>
    public DbSet<OssUserImageData> OssUserImageData { get; set; }
    /// <summary>
    /// 存储配置表
    /// </summary>
    public DbSet<StorageConfig> StorageConfig { get; set; }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        /* Configure your own entities here */
    }
}
