using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Requests.Admin
{
    public class AuthorizeUserRolesRequest
    {
        public long UserId { get; set; }

        public long RoleId { get; set; }

    }
}
