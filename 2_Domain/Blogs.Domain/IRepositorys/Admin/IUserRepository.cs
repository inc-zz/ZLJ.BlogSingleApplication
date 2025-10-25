using Blogs.Domain.Entity.Admin;
using Blogs.Domain.IRepositorys;

namespace Blogs.Domain.IRepositorys.Admin
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserRepository : IBaseRepository<SysUser>
    {
        /// <summary>
        /// 用户角色授权
        /// </summary>
        /// <param name="adminUser"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<bool> UserRoleAuth(SysUser adminUser, long roleId);
        /// <summary>
        /// 更新登录时间
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task UpdateLoginTimeAsync(long userId);
        /// <summary>
        /// 查询用户分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchTerm"></param>
        /// <param name="isActive"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<(IEnumerable<SysUser> Users, int TotalCount)> GetUsersForListingAsync(
        int pageIndex,
        int pageSize,
        string searchTerm = null,
        bool? isActive = null,
        CancellationToken cancellationToken = default);

        Task<SysUser> GetByUserNameAsync(string userName);
        Task<bool> LockUserAsync(long userId, DateTime unlockTime);
        Task<bool> IncrementLoginFailedCountAsync(long userId);
        Task<bool> ResetLoginFailedCountAsync(long userId);
        /// <summary>
        /// 更新登录时间和IP地址
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ipAddress"></param>
        /// <param name="loginTime"></param>
        /// <returns></returns>
        Task<bool> UpdateLastLoginInfoAsync(long userId, string ipAddress, DateTime loginTime);
    }
}
