using Blogs.Core.Entity.Blogs;
using Blogs.Domain.Entity.Admin;
using Blogs.Domain.IRepositorys.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Infrastructure.Repositorys.Blogs
{
    public class AppUserRepository : SimpleClient<BlogsUser>, IAppUserRepository
    {
        private readonly SqlSugarDbContext dbContext;
        public AppUserRepository()
        {
            dbContext = new SqlSugarDbContext();
            base.Context = dbContext.DbContext;
        }


        public async Task<bool> UpdateLastLoginInfoAsync(long userId, string ipAddress, DateTime loginTime)
        {
            var user = await dbContext.DbContext.Queryable<BlogsUser>().FirstAsync(it => it.Id == userId);
            if (user == null)
            {
                return false;
            }
            user.LastLoginTime = loginTime;
            user.LastLoginIp = ipAddress;
            var result = await dbContext.DbContext.Updateable(user).ExecuteCommandAsync();
            return result > 0;
        }
    }
}
