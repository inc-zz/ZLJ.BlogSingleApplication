using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.ResponseDto.Admin
{
    public class UserRoleDto
    {
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
        public bool IsAssigned { get; set; }
    }

}
