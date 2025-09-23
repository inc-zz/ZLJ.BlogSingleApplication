using Blogs.AppServices.Commands.Admin.SysDepartment;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Domain.ModelValidator.Department
{

    /// <summary>
    /// 
    /// </summary>
    public class DeleteDepartmentCommandValidation:DepartmentValidatorCommand<DeleteDepartmentCommand>
    {
        /// <summary>
        /// 
        /// </summary>
        public DeleteDepartmentCommandValidation()
        {
            ValidateId();
        }
    }
}
