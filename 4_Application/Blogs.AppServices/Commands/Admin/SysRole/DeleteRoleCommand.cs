using Blogs.Domain.ValueValidator.Role;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Commands.Admin.SysRole 
{

    /// <summary>
    /// 
    /// </summary>
    public class DeleteRoleCommand : RoleCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public DeleteRoleCommand(long id)
        {
            Id = id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            ValidationResult = new DeleteRoleCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
