using Blogs.Domain.ModelValidator.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Commands.Admin.SysUser
{

    /// <summary>
    /// 
    /// </summary>
    public class DeleteUserCommand:UserCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public DeleteUserCommand(long id)
        {
            Id = id;
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
