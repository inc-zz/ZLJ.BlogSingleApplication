using Blogs.Domain.ModelValidator.Department;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Commands.Admin.SysDepartment
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteDepartmentCommand:DepartmentCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public DeleteDepartmentCommand(long id)
        {
            Id = id;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
             ValidationResult = new DeleteDepartmentCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
