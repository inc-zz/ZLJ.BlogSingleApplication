using Blogs.AppServices.Commands.Admin.SysRole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.ModelValidator.Admin.Role
{

    /// <summary>
    /// 角色菜单授权验证
    /// </summary>
    public class RoleMenuAuthCommandValidation:RoleValidatorCommand<RoleCommand>
    {
        /// <summary>
        /// 
        /// </summary>
        public RoleMenuAuthCommandValidation()
        {
            ValidateCode();
            ValidateRoleMenus();
        }

    }
}
