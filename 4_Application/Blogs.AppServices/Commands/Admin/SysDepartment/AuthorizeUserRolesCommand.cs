using Blogs.AppServices.Requests.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Admin.SysDepartment
{
    public class AuthorizeUserRolesCommand : AuthorizeCommand
    {

        public AuthorizeUserRolesCommand(AuthorizeUserRolesRequest request)
        {
            
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
