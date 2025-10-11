using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.ResponseDto.Admin
{
    public class RoleModulePermissionDto
    {
        public long ModuleId { get; set; }
        public string? ModuleName { get; set; }
        public string? ModuleCode { get; set; }
        public long ParentId { get; set; }
        public bool IsAuthorized { get; set; }
        public string? Permissions { get; set; }
    }
}
