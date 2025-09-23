using Blogs.Core.Entity.Admin;
using Blogs.Domain.Entity.Admin;
using Blogs.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Domain.ServiceValidator.Services
{

    /// <summary>
    /// 用户验证服务
    /// </summary>
    public class UserValidatorService : SqlSugarDbContext, IUserValidatorService
    {

        /// <summary>
        /// 验证用户是否存在
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool ExistsUser(string account)
        {
            return DbContext.Queryable<SysUser>().Any(x => x.UserName == account);
        }
        /// <summary>
        /// 验证用户是否存在租户
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool ExistsTenant(string account)
        {
            return DbContext.Queryable<SysUser>().Any(x => x.UserName == account);
        }

    }
}
