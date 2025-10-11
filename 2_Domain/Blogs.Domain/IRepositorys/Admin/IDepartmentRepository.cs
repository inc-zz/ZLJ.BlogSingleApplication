using Blogs.Domain.Entity.Admin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Domain.IRepositorys.Admin
{

    /// <summary>
    /// 部门仓储接口
    /// </summary>
    public interface IDepartmentRepository : IBaseRepository<SysDepartment>
    {

        Task<List<SysDepartment>> GetDepartmentTreeAsync();

    }
}
