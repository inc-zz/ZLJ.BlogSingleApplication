using Blogs.AppServices.Commands.Admin.SysUser;
using Blogs.AppServices.ModelValidator.App.User;
using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Blogs.User
{
    /// <summary>
    /// App用户登录命令
    /// </summary>
    public class AppUserLoginCommand : AppUserCommand, IRequest<ResultObject>
    {

        public AppUserLoginCommand(string account, string password)
        {
            this.Account = account;
            this.Password = password;
        }

        public override bool IsValid()
        {
            ValidationResult = new AppUserLoginCommandValidattion().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
