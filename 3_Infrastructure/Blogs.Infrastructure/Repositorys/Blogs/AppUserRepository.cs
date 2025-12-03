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

        /// <summary>
        /// 查询App用户分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchTerm"></param>
        /// <param name="isDeleted"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<BlogsUser> Users, int TotalCount)> GetAppUserListAsync(
        int pageIndex,
        int pageSize,
        string searchTerm = null,
        int? isDeleted = null,
        CancellationToken cancellationToken = default)
        {
            var query = base.Context.Queryable<BlogsUser>();
            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(u => u.Account.Contains(searchTerm) || u.Email.Contains(searchTerm));

            if (isDeleted.HasValue)
                query = query.Where(u => u.IsDeleted == isDeleted);

            var totalCount = await query.CountAsync(cancellationToken);
            var skip = (pageIndex - 1) * pageSize;
            var users = await query.Skip(skip).Take(pageSize).ToListAsync(cancellationToken);
            return (users, totalCount);

        }

    }
}
