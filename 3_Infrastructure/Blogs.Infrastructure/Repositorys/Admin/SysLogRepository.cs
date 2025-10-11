using Blogs.Core.Entity.Admin;
using Blogs.Domain.Entity.Admin;
using Blogs.Domain.IRepositorys.Admin;

namespace Blogs.Infrastructure.Repositorys.Admin
{

    /// <summary>
    /// 系统日志仓储层
    /// </summary>
    public class SysLogRepository : SimpleClient<SysLog>, ISysLogRepository
    {


    }
}
