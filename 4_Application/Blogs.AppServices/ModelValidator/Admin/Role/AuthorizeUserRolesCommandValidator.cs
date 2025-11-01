using Blogs.AppServices.Commands.Admin.SysDepartment;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.ModelValidator.Admin.Role
{
    public class AuthorizeUserRolesCommandValidator : AbstractValidator<AuthorizeUserRolesCommand>
    {
        public AuthorizeUserRolesCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("用户ID必须大于0");

            RuleFor(x => x.RoleId)
                .NotNull().WithMessage("角色ID列表不能为空");
        }
    }
}
