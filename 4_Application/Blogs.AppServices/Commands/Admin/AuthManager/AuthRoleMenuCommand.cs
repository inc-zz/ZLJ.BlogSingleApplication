using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Admin.AuthManager
{
    public class AuthRoleMenuCommand : AuthCommand
    {
        public AuthRoleMenuCommand()
        {
            
        }
        public override bool IsValid()
        {
            return this.RoleMenu != null && this.RoleMenu.Count > 0;
        }
    }
}
