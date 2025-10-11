using Blogs.AppServices.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.Admin
{
    /// <summary>
    /// 获取部门树查询
    /// </summary>
    public class GetDepartmentTreeQuery : IRequest<List<DepartmentTreeDto>>
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        public string? Name { get; set; }


    }
}
