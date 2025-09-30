using Blogs.AppServices.ModelValidator.User;
using Blogs.AppServices.Requests.Admin;
using Blogs.Domain.ValueValidator.User;
using Microsoft.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Admin.SysUser
{
    /// <summary>
    /// 
    /// </summary>
    public class ResetPasswordCommand : UserCommand
    {
        public ResetPasswordCommand(ChangePasswordRequest param)
        {
            this.Password = param.Password;
            this.OldPassword = param.OldPassword;
        }

        public override bool IsValid()
        {
            ValidationResult = new ResetPasswordCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
