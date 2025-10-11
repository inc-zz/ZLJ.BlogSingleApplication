using Blogs.AppServices.ModelValidator.Admin.Role;
using Blogs.AppServices.Requests.Admin;
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
        public UpdateRoleCommand(UpdateSysRoleParam request)
        {
            Id = request.Id;
            Name = request.Name;
            Code = request.Code;
            Sort = request.Sort;
            Remark = request.Remark;
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
