using Blogs.AppServices.Commands.Admin.SysRole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.ModelValidator.Admin.Role
{

    /// <summary>
    /// 编辑角色数据验证
    /// </summary>
    public class UpdateRoleCommandValidation : RoleValidatorCommand<RoleCommand>
    {
        /// <summary>
        /// 
        /// </summary>
        public UpdateRoleCommandValidation()
        {
            ValidateId();
            ValidateName();
            ValidateCode();
        }
    }
}
