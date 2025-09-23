using Blogs.Core;
using Blogs.Core.Entity.Admin;
using Blogs.Core.Models;
using Blogs.Domain.Entity.Admin;
using Blogs.Domain.IRepositorys.Admin;

namespace Blogs.Infrastructure.Repositorys.Admin
{
    /// <summary>
    /// 用户仓储实现
    /// </summary>
    public class UserRepository : SimpleClient<SysUser>, IUserRepository
    {
        private readonly SqlSugarDbContext dbContext;
        public UserRepository()
        {
            dbContext = new SqlSugarDbContext();
            base.Context = dbContext.DbContext;
        }

        /// <summary>
        /// 用户角色授权
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<bool> UserRoleAuth(long userId, long roleId)
        {
            var isAny = await base.Context.Queryable<SysUserRoleRelation>().Where(it => it.UserId == userId
            && it.RoleId == roleId).AnyAsync();
            if (!isAny)
            {
                var userRole = new SysUserRoleRelation
                {
                    RoleId = roleId,
                    UserId = userId
                };
                var res = await base.Context.Insertable(userRole).ExecuteCommandAsync();
                isAny = res > 0;
            }
            return isAny;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task UpdateLoginTimeAsync(long userId)
        {
            var user = await base.Context.Queryable<SysUser>().Where(it => it.Id == userId).FirstAsync();
            if (user != null)
            {
                user.LastLoginTime = DateTime.Now;
                base.Context.Updateable(user);
            }
        }

        /// <summary>
        /// 查询用户分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchTerm"></param>
        /// <param name="isActive"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<SysUser> Users, int TotalCount)> GetUsersForListingAsync(
        int pageIndex,
        int pageSize,
        string searchTerm = null,
        bool? isActive = null,
        CancellationToken cancellationToken = default)
        {
            // 使用SqlSugar实现复杂查询
            var query = base.Context.Queryable<SysUser>();

            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(u => u.UserName.Contains(searchTerm) || u.Email.Contains(searchTerm));

            if (isActive.HasValue)
                query = query.Where(u => u.IsDeleted == isActive.Value);

            var totalCount = await query.CountAsync(cancellationToken);
            var skip = (pageIndex - 1) * pageSize;
            var users = await query.Skip(skip).Take(pageSize).ToListAsync(cancellationToken);

            return (users, totalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<SysUser> GetByUserNameAsync(string userName)
        {
            var conn = base.Context.CurrentConnectionConfig.ConnectionString;
            try
            {
                var userList = await base.Context.Queryable<SysUser>().ToListAsync();

                var conn2 = dbContext.DbContext.CurrentConnectionConfig.ConnectionString;
                var user = await dbContext.DbContext.Queryable<SysUser>()
                    .Where(it => it.UserName == userName).FirstAsync();
                return user;

            }
            catch (Exception e)
            {

                throw;
            }
        }

        public Task<bool> LockUserAsync(long userId, DateTime unlockTime)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IncrementLoginFailedCountAsync(long userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> ResetLoginFailedCountAsync(long userId)
        {
            var res = await dbContext.DbContext.Updateable<SysUser>()
                 .SetColumns(it => new SysUser { AccessFailedCount = 0 })
                 .Where(wt => wt.Id == userId).ExecuteCommandAsync();
            return res > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ipAddress"></param>
        /// <param name="loginTime"></param>
        /// <returns></returns>
        public async Task<bool> UpdateLastLoginInfoAsync(long userId, string ipAddress, DateTime loginTime)
        {
            var res = await dbContext.DbContext.Updateable<SysUser>()
               .SetColumns(it => new SysUser { LastLoginTime = loginTime })
               .Where(wt => wt.Id == userId).ExecuteCommandAsync();
            return res > 0;
        }
    }
}
