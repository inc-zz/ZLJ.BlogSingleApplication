using Blogs.AppServices.Queries.ResponseDto.Admin;
using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.Admin
{
    public class GetRoleMenuAuthQuery : IRequest<ResultObject<List<RoleMenuDto>>>
    {
        public string[] RoleIds { get; set; }

        public GetRoleMenuAuthQuery(string roleIds)
        {
            RoleIds = roleIds?.Split(",").ToArray();   
        }

    }
}
