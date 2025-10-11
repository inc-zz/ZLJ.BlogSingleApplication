using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Requests.Admin
{
    public class AddRoleRequest
    {
        public int IsSystem { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Remark { get; set; }
    }
}
