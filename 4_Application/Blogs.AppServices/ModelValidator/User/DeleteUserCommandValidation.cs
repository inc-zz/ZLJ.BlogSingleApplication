using Blogs.AppServices.Commands.Admin.SysUser;
using Blogs.Domain.ValueValidator.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Domain.ModelValidator.User
{

    /// <summary>
    /// 
    /// </summary>
    public class DeleteUserCommandValidation:UserValidatorCommand<UserCommand>
    {
        /// <summary>
        /// 
        /// </summary>
        public DeleteUserCommandValidation()
        {
            ValidateId();
        }
    }
}
