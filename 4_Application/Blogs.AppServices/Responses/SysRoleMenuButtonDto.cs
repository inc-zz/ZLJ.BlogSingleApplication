using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Responses
{
    public class SysRoleMenuButtonDto
    {

        public long RoleId { get; set; }
        public long MenuId { get; set; }
        public long ButtonId { get; set; }
        public string? ButtonName { get; set; }
        public string? ButtonCode { get; set; }
    }
}
