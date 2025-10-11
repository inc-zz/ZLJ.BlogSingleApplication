using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.Admin
{
    public class GetDepartmentRequest : PageParam
    {
        /// <summary>
        /// 部门Id
        /// </summary>
        public long DepId { get; set; }


    }
}
