using Blogs.AppServices.Commands.Admin.AuthManager;
using Blogs.AppServices.Commands.Admin.SysUser;
using Blogs.AppServices.Requests.Admin;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.ModelValidator.Admin.AuthManager
{
    /// <summary>
    /// 授权模块验证
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AuthValidatorCommand<T> : AbstractValidator<T> where T : AuthCommand
    {

        /// <summary>
        /// 验证角色Id
        /// </summary>
        protected void ValidateRoleId()
        {
            RuleFor(x => x.RoleId).NotNull().NotEqual(0).WithMessage("角色Id不能为空");
        }

        /// <summary>
        /// 验证角色菜单
        /// </summary>
        protected void ValidateRoleMenus()
        {
            // 验证 RoleMenus 列表本身不能为 null
            RuleFor(x => x.RoleMenus)
                .NotNull()
                .WithMessage("角色菜单列表不能为null");

            // 验证 RoleMenus 列表不能为空（至少需要一个菜单）
            RuleFor(x => x.RoleMenus)
                .NotEmpty()
                .WithMessage("至少需要一个菜单授权")
                .When(x => x.RoleMenus != null);

            // 验证每个菜单项
            RuleForEach(x => x.RoleMenus)
                .SetValidator(new AuthMenuRequestValidator())
                .When(x => x.RoleMenus != null);
        }
    }
    /// <summary>
    /// 菜单请求验证器
    /// </summary>
    public class AuthMenuRequestValidator : AbstractValidator<AuthMenuRequest>
    {
        public AuthMenuRequestValidator()
        {
            ValidateMenuId();
            ValidateButtonIds();
        }

        /// <summary>
        /// 验证菜单Id
        /// </summary>
        private void ValidateMenuId()
        {
            RuleFor(x => x.MenuId)
                .NotNull()
                .NotEqual(0)
                .WithMessage("菜单Id不能为空");
        }

        /// <summary>
        /// 验证按钮Ids
        /// </summary>
        private void ValidateButtonIds()
        {
            // 验证 ButtonIds 数组不能为 null
            RuleFor(x => x.ButtonIds)
                .NotNull()
                .WithMessage("按钮ID数组不能为null");

            // 验证 ButtonIds 数组中的每个元素（如果业务需要）
            RuleFor(x => x.ButtonIds)
                .Must(ids => ids == null || ids.All(id => id > 0))
                .WithMessage("按钮ID必须大于0")
                .When(x => x.ButtonIds != null);

        }
    }
}
