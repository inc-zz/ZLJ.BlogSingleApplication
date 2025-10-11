using Blogs.AppServices.Commands.Blogs.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.ModelValidator.App.User
{
    /// <summary>
    /// App用户登录
    /// </summary>
    public class AppUserLoginCommandValidattion : AppUserValidatorCommand<AppUserCommand>
    {

        public AppUserLoginCommandValidattion()
        {
            ValidateAccount();
        }

    }
}
