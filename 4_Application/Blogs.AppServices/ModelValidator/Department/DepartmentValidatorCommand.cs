using Blogs.AppServices.Commands.Admin.SysDepartment;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Domain.ModelValidator.Department
{

    /// <summary>
    /// 部门数据验证
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DepartmentValidatorCommand<T> : AbstractValidator<T> where T: DepartmentCommand 
    {

        /// <summary>
        /// 部门Id
        /// </summary>
        protected void ValidateId()
        {
            RuleFor(v => v.Id).NotEqual(0).WithName("部门Id");
        }
        /// <summary>
        /// 部门名称
        /// </summary>
        protected void ValidateName()
        {
            RuleFor(v => v.Name).NotEmpty().WithName("部门名称").NotNull();
        }
       
        /// <summary>
        /// 部门简称
        /// </summary>
        protected void ValidateAbbreviation()
        {
            RuleFor(x => x.Abbreviation).NotEmpty().WithName("部门简称");
        } 

    }
}
