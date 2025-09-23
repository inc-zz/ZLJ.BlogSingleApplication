using Blogs.Domain.ValueValidator.User;
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
        public UserRoleAuthCommand(long userId, long roleId)
        {
            Id = userId;
            RoleId = roleId;
            //this.UpdateUser = CurrentUserContext.Instance.Account;
            //this.UpdateUserName = CurrentUserContext.Instance.TrueName;
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
