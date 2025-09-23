using Blogs.AppServices.Commands.Admin.SysRole;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Domain.ValueValidator.Role
{

    /// <summary>
    /// 角色数据验证
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RoleValidatorCommand<T> : AbstractValidator<T> where T : RoleCommand
    {
        /// <summary>
        /// 
        /// </summary>
        public RoleValidatorCommand()
        {

        }
        /// <summary>
        /// 验证Id
        /// </summary>
        protected void ValidateId()
        {
            RuleFor(x => x.Id).NotEqual(0).WithMessage("Id不能为空");
        }
        /// <summary>
        /// 租户Id
        /// </summary>
        protected void ValidateTenantId()
        {
            RuleFor(x => x.TenantId).NotEqual(0).WithMessage("租户Id不能为空");
        }
        /// <summary>
        /// 验证角色组
        /// </summary>
        protected void ValidateParentId()
        {
            RuleFor(x => x.ParentId).NotNull().NotEqual(0).WithMessage("角色组不能为空");
        }
        /// <summary>
        /// 验证角色名称
        /// </summary>
        protected void ValidateName()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull().WithMessage("角色名称不能为空");
        }
        /// <summary>
        /// 验证部门Id
        /// </summary>
        protected void ValidateDepartmentId()
        {
            RuleFor(x => x.DepartmentId).NotNull().NotEmpty().WithMessage("部门Id不能为空");
        }
        /// <summary>
        /// 验证部门名称
        /// </summary>
        protected void ValidateDepartmentName()
        {
            RuleFor(x => x.DepartmentName).NotNull().NotEmpty().WithMessage("部门不能为空");
        }
        ///<summary>
        ///  角色编号
        ///</summary>
        protected void ValidateCode()
        {
            RuleFor(x => x.Code).NotNull().NotEmpty().WithMessage("角色编号不能为空");
        }
        ///<summary>
        /// 菜单
        /// </summary>
        protected void ValidateMenuId()
        {
            RuleFor(x => x.MenuId).NotEqual(0).WithMessage("菜单Id不能为空");
        }
        /// <summary>
        /// 菜单Code
        /// </summary>
        protected void ValidaateMenuCode()
        {
            RuleFor(x => x.MenuCode).NotEmpty().WithMessage("菜单Code不能为空");
        }
        /// <summary>
        /// 菜单按钮
        /// </summary>
        protected void ValidateButtonCodes()
        {
            RuleFor(x => x.ButtonCodes).NotEmpty().WithMessage("菜单按钮不能为空");
        }

    }
}
