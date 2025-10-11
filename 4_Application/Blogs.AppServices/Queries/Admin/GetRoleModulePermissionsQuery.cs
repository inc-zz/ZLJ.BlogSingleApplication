using Blogs.AppServices.Queries.ResponseDto.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.Admin
{
    public class GetRoleModulePermissionsQuery : IRequest<List<RoleModulePermissionDto>>
    {
        public long RoleId { get; set; }
    }
}
