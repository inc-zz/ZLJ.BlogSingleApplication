using Blogs.Core.Entity.Blogs;
using Blogs.Domain.Entity.Admin;
using Blogs.Domain.ServiceValidator.Admin;
using Blogs.Domain.ServiceValidator.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Infrastructure.ValidationServer.App
{
    /// <summary>
    /// App用户验证模块
    /// </summary>
    public class AppUserValidatorService : SqlSugarDbContext, IAppUserValidatorService
    {
        public bool ExistsUser(string account)
        {
            return DbContext.Queryable<BlogsUser>().Any(x => x.Account == account);
        }

    }
}
