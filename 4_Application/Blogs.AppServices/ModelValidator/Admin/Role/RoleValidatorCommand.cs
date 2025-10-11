using Blogs.AppServices.Commands.Admin.SysRole;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.ModelValidator.Admin.Role
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
     
        ///<summary>
        ///  角色编号
        ///</summary>
        protected void ValidateCode()
        {
            RuleFor(x => x.Code).NotNull().NotEmpty().WithMessage("角色编号不能为空");
        }

        /// <summary>
        /// 角色菜单
        /// </summary>
        protected void ValidateRoleMenus()
        {
            RuleFor(x => x.RoleMenus)
                .NotNull().WithMessage("角色菜单不能为空")
                .NotEmpty().WithMessage("至少需要一个角色菜单配置")
                .Must(HaveValidRoleMenus).WithMessage("角色菜单配置包含无效数据");

            // 对集合中的每个元素进行验证
            RuleForEach(x => x.RoleMenus)
                .ChildRules(roleMenu =>
                {
                    roleMenu.RuleFor(x => x.MenuId)
                        .GreaterThan(0).WithMessage("菜单ID必须大于0");

                    roleMenu.RuleFor(x => x.ButtonCodes)
                        .NotEmpty().WithMessage("菜单按钮代码不能为空")
                        .NotNull().WithMessage("菜单按钮代码不能为null");
                });
        }
        private bool HaveValidRoleMenus(List<RoleMenu> roleMenus)
        {
            return roleMenus != null &&
                   roleMenus.All(rm => rm != null &&
                                     rm.MenuId > 0 &&
                                     !string.IsNullOrWhiteSpace(rm.ButtonCodes));
        }
    }
}
