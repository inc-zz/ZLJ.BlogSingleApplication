using Blogs.AppServices.Commands.Admin.SysDepartment;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Domain.ModelValidator.Department
{
    /// <summary>
    /// 创建部门
    /// </summary>
    public class CreateDepartmentCommandValidation : DepartmentValidatorCommand<DepartmentCommand>
    {
        /// <summary>
        /// 创建部门数据验证
        /// </summary>
        public CreateDepartmentCommandValidation()
        {
            ValidateName();
        }

    }
}
