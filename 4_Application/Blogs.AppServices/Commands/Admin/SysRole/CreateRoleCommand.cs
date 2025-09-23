using Blogs.Domain.ValueValidator.Role;
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
        public CreateRoleCommand(long tenantId,int isSystem, long departmentId, string name, string code,int sort,string summary)
        {
            TenantId = tenantId;
            IsSystem = isSystem;
            DepartmentId = departmentId;
            Name = name;
            Code = code;
            Sort = sort;
            Summary = summary;
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
