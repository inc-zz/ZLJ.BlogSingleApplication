using Blogs.AppServices.Commands.Admin.SysUser;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Domain.ValueValidator.User
{
    /// <summary>
    /// 用户租户授权
    /// </summary>
    public class UserTenantAuthCommandValidation : UserValidatorCommand<UserCommand>
    {

        /// <summary>
        /// 
        /// </summary>
        public UserTenantAuthCommandValidation()
        {
            ValidateId();
            ValidateTenantId();
        }
    }
}
