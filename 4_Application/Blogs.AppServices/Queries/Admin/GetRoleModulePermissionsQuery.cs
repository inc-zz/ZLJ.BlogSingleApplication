using Blogs.AppServices.Queries.ResponseDto.Admin;
using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.Admin
{
    public class GetRoleModulePermissionsQuery : IRequest<ResultObject<List<RoleMenuPermissionDto>>>
    {
        public long RoleId { get; set; }
    }
}
