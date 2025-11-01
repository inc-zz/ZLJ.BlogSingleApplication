using Blogs.Core.Entity.Admin;
using Blogs.Domain.Entity.Admin;
using Blogs.Domain.IRepositorys.Admin;

namespace Blogs.Infrastructure.Repositorys.Admin
{

    /// <summary>
    /// 系统权限表仓储
    /// </summary>
    public class PermissionsRepository: SimpleClient<SysRoleMenuAuth>,IPermissionsRepository
    {



    }
}
