using Blogs.AppServices.Queries.ResponseDto;
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
    public class GetAllRolesQuery : PageParam, IRequest<Dictionary<string, string>>
    {
        public string? Name { get; set; }
    }
}
