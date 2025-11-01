using Blogs.AppServices.ModelValidator.Admin.User;
using Blogs.AppServices.Requests.Admin;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Commands.Admin.SysUser
{

    /// <summary>
    /// 用户角色授权
    /// </summary>
    public class UserRoleAuthCommand : UserCommand
    {
        /// <summary>
        /// 
        /// </summary>
        public UserRoleAuthCommand(SetUserRolesRequest request)
        {
            Id = request.UserId;
            UserRoleJson = request.UserRoleJson;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            ValidationResult = new UserRoleAuthCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

    }
}
