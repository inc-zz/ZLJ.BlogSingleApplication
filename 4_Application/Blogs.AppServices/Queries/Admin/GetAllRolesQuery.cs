using Blogs.AppServices.Queries.ResponseDto;
using Blogs.AppServices.Queries.ResponseDto.Admin;
using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.Admin
{
    /// <summary>
    /// 所有角色
    /// </summary>
    public class GetAllRolesQuery : PageParam, IRequest<ResultObject<List<DropDownlistDto>>>
    {
        public string? Name { get; set; }
    }
}
