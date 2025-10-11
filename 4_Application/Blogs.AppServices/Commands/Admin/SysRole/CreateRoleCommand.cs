using Blogs.AppServices.ModelValidator.Admin.Role;
using Blogs.AppServices.Requests.Admin;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Commands.Admin.SysRole
{

    /// <summary>
    /// 创建角色命令
    /// </summary>
    public class CreateRoleCommand:RoleCommand
    {
        /// <summary>
        /// 
        /// </summary>
        public CreateRoleCommand(AddRoleRequest request )
        {
            IsSystem = request.IsSystem;
            Name = request.Name;
            Code = request.Code;
        }

        /// <summary>
        /// 数据验证
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            ValidationResult = new CreateRoleCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

    }
}
