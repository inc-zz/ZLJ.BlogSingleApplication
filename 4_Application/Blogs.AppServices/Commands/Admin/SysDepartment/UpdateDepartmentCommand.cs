using Blogs.Domain.ModelValidator.Department;
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
        public UpdateDepartmentCommand(long id, long parentId, string name, string abbreviation, int sort, string remark)
        {
            Id = id;
            ParentId = parentId;
            Name = name;
            //Code = code;
            Abbreviation = abbreviation;
            Sort = sort;
            Remark = remark;
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
