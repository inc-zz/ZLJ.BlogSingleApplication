using Blogs.AppServices.Commands.Admin.SysRole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.ModelValidator.Admin.Role
{

    /// <summary>
    /// 角色菜单按钮授权 
    /// </summary>
    public class RoleMenuButtonsAuthCommand : RoleValidatorCommand<RoleCommand>
    {
        /// <summary>
        /// 
        /// </summary>
        public RoleMenuButtonsAuthCommand()
        {
            ValidateId();

        }
    }
}
