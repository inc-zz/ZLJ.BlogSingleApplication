using Blogs.Domain.ValueValidator.Role;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Commands.Admin.SysRole
{

    /// <summary>
    /// 修改角色
    /// </summary>
    public class UpdateRoleCommand:RoleCommand
    {
        
        /// <summary>
        /// 
        /// </summary>
        public UpdateRoleCommand(long id, string name,string code,int sort,string summary)
        {
            Id = id;
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
            ValidationResult = new UpdateRoleCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

    }
}
