using Blogs.Domain.ModelValidator.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Commands.Admin.SysUser
{

    /// <summary>
    /// 
    /// </summary>
    public class LoginOutCommand: UserCommand
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        public LoginOutCommand(long userId)
        {
            Id = userId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            ValidationResult = new DeleteUserCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
