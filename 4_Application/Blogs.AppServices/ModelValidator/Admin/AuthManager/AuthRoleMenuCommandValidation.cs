using Blogs.AppServices.Commands.Admin.AuthManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.ModelValidator.Admin.AuthManager
{
    public class AuthRoleMenuCommandValidation : AuthValidatorCommand<AuthCommand>
    {

        public AuthRoleMenuCommandValidation()
        {
            ValidateRoleId();
            ValidateRoleMenus();
        }


    }
}
