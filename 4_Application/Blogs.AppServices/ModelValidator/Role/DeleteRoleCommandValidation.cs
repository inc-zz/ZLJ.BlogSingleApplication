using Blogs.AppServices.Commands.Admin.SysRole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Domain.ValueValidator.Role
{

    /// <summary>
    /// 删除角色验证
    /// </summary>
    public class DeleteRoleCommandValidation:RoleValidatorCommand<RoleCommand>
    {
        /// <summary>
        /// 
        /// </summary>
        public DeleteRoleCommandValidation()
        {
            ValidateId(); 
        }

    }
}
