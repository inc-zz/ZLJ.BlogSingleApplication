using Blogs.AppServices.ModelValidator.Admin.AuthManager;
using Blogs.AppServices.Requests.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Admin.AuthManager
{
    public class AuthRoleMenuCommand : AuthCommand
    {
        public AuthRoleMenuCommand(AuthRoleMenuRequest request)
        {
            this.RoleId = request.RoleId;
            this.RoleMenus = request.RoleMenus;
            
        }
        public override bool IsValid()
        {
           var result = new AuthRoleMenuCommandValidation().Validate(this); 
            return result.IsValid;
        }
    }
}
