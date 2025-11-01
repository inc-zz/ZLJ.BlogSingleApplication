using Blogs.Core;
using Blogs.Core.DtoModel.Admin;
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
        public async Task<bool> UserRoleAuth(SysUser adminUser,long roleId)
        {
            var isAny = await base.Context.Queryable<SysUserRoleRelation>().Where(it => it.UserId == adminUser.Id
            && it.RoleId == roleId).AnyAsync();
            if (!isAny)
            {
                var userRole = new SysUserRoleRelation
                {
                    RoleId = roleId,
                    UserId = adminUser.Id
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
