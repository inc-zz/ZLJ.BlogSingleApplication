using Blogs.AppServices.ModelValidator.Admin.Role;
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
            this.RoleId = request.RoleId;
            this.UserId = request.UserId;
        }

        public override bool IsValid()
        {
            ValidationResult = new AuthorizeUserRolesCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
