using Blogs.Core.Entity.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Domain.IRepositorys.Blogs
{
    public interface IAppUserRepository : IBaseRepository<BlogsUser>
    {
        /// <summary>
        /// 更新登录时间和IP地址
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ipAddress"></param>
        /// <param name="loginTime"></param>
        /// <returns></returns>
        Task<bool> UpdateLastLoginInfoAsync(long userId, string ipAddress, DateTime loginTime);

        /// <summary>
        /// 查询App用户分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchTerm"></param>
        /// <param name="isDeleted"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<(IEnumerable<BlogsUser> Users, int TotalCount)> GetAppUserListAsync(
        int pageIndex,
        int pageSize,
        string searchTerm = null,
        int? isDeleted = null,
        CancellationToken cancellationToken = default);

    }
}
