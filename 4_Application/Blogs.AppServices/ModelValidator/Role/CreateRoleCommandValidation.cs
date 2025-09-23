using Blogs.AppServices.Commands.Admin.SysRole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Domain.ValueValidator.Role
{

    /// <summary>
    /// 创建角色数据验证
    /// </summary>
    public class CreateRoleCommandValidation:RoleValidatorCommand<RoleCommand>
    {
        /// <summary>
        /// 
        /// </summary>
        public CreateRoleCommandValidation()
        {
            ValidateName();
            ValidateCode();
        }

    }
}
