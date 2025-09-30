using Blogs.AppServices.Requests.Admin;
using Blogs.Domain.ModelValidator.Department;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Commands.Admin.SysDepartment
{

    /// <summary>
    /// 创建部门命令
    /// </summary>
    public class CreateDepartmentCommand : DepartmentCommand
    {

        /// <summary>
        /// 初始化部门
        /// </summary>
        public CreateDepartmentCommand(AddDepartmentRequest param)
        {
            ParentId = param.ParentId;
            Name = param.Name;
            Abbreviation = param.Abbreviation;
            Sort = param.Sort;
            Remark = param.Summary;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            ValidationResult = new CreateDepartmentCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
