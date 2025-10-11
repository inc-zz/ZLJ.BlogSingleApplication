using Blogs.Core.Entity.Admin;
using Blogs.Domain.Entity.Admin;
using Blogs.Domain.IRepositorys.Admin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Infrastructure.Repositorys.Admin
{

    /// <summary>
    /// 部门仓储接口
    /// </summary>
    public class DepartmentRepository : SimpleClient<SysDepartment>, IDepartmentRepository
    {
        private readonly SqlSugarClient dbContext;
        public DepartmentRepository()
        {
            dbContext = new SqlSugarDbContext().DbContext;

        }

        public async Task<List<SysDepartment>> GetDepartmentTreeAsync()
        {
            var list = await dbContext.Queryable<SysDepartment>()
                .Where(it => it.IsDeleted == 0)
                .ToTreeAsync(it => it.Children, it => it.ParentId, 0);
            return list;
        }
    }
}
