using Blogs.AppServices.Commands.Admin.SysUser;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Domain.ValueValidator.User
{

    /// <summary>
    /// 新增用户数据验证
    /// </summary>
    public class CreateUserCommandValidation: UserValidatorCommand<UserCommand>
    {
        /// <summary>
        /// 
        /// </summary>
        public CreateUserCommandValidation()
        {
            ValidateName();
            ValidatePassword();
            ValidateDepartment();
            ValidateAccountExists();
        }
    }
}
