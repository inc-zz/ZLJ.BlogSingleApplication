using Blogs.AppServices.Commands.Admin.SysUser;
using Blogs.Domain.ValueValidator.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.ModelValidator.User
{
    public class ResetPasswordCommandValidation : UserValidatorCommand<UserCommand>
    {
        public ResetPasswordCommandValidation()
        {
            ValidatePassword();
        }

    }
}
