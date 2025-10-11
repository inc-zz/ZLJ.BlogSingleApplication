using Blogs.AppServices.ModelValidator.Admin.Department;
using Blogs.AppServices.Requests.Admin;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Commands.Admin.SysDepartment
{

    /// <summary>
    /// 修改部门
    /// </summary>
    public class UpdateDepartmentCommand : DepartmentCommand
    {

        /// <summary>
        /// 修改部门初始化参数
        /// </summary>
        public UpdateDepartmentCommand(UpdateDepartmentRequest request)
        {
            Id = request.Id;
            ParentId = request.ParentId;
            Name = request.Name;
            //Code = code;
            Abbreviation = request.Abbreviation;
            Sort = request.Sort;
            Remark = request.Summary;
        }

        /// <summary>
        /// 数据验证结果
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            ValidationResult = new UpdateDepartmentCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
