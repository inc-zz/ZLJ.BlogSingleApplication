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

    }
}
