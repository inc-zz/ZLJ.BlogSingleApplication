﻿using Blogs.AppServices.Commands.Admin.SysUser;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.ModelValidator.Admin.User
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
