using Blogs.AppServices.Commands.Admin.SysUser;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.ModelValidator.Admin.User
{

    /// <summary>
    /// 用户角色授权
    /// </summary>
    public class UserRoleAuthCommandValidation : UserValidatorCommand<UserCommand>
    {

        /// <summary>
        /// 
        /// </summary>
        public UserRoleAuthCommandValidation()
        {
            ValidateId();
            ValidateRoleIds();
        }
    }
}
