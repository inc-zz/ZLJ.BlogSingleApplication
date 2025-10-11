using Blogs.AppServices.Commands.Admin.SysDepartment;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.ModelValidator.Admin.Department
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
