using Blogs.AppServices.Commands.Admin.SysRole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Domain.ValueValidator.Role
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
            ValidaateMenuCode();
        }

    }
}
