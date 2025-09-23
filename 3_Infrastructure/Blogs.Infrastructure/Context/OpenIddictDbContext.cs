using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Infrastructure.Context
{
    public class OpenIddictDbContext : DbContext
    {
        public OpenIddictDbContext(DbContextOptions<OpenIddictDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 配置 OpenIddict 实体
            builder.UseOpenIddict();
        }
    }
}
