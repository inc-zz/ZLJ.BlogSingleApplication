using Blogs.AppServices.Commands.Admin.SysMenu;
using FluentValidation;
namespace Blogs.AppServices.ModelValidator.Admin.Menu
{
    /// <summary>
    /// 菜单数据验证
    /// </summary>
    public abstract class MenuValidatorCommand<T> : AbstractValidator<T> where T : MenuCommand
    {
        /// <summary>
        /// 
        /// </summary>
        public MenuValidatorCommand()
        {
        }

        /// <summary>
        /// 验证菜单Id
        /// </summary>
        protected void ValidateId()
        {
            RuleFor(v => v.Id).NotEqual(0).WithMessage("菜单Id不能为空");
        }

        /// <summary>
        /// 验证菜单名称
        /// </summary>
        protected void ValidateName()
        {
            RuleFor(v => v.Name).NotEmpty().WithName("菜单名称").MaximumLength(15).WithMessage("长度不能超过15");
        }

        /// <summary>
        /// 验证菜单Code
        /// </summary>
        protected void ValidateCode()
        {
            RuleFor(v => v.Type).NotEmpty().WithMessage("菜单类型不能为空");
        }
        /// <summary>
        /// 验证菜单Url
        /// </summary>
        protected void ValidateUrl()
        {
            RuleFor(v => v.Url).NotEmpty().WithName("菜单Url");
        }
        /// <summary>
        /// 验证租户
        /// </summary>
        protected void ValidateTenant()
        {
            RuleFor(v => v.TenantId).NotEqual(0).WithMessage("租户不能为空");
        }
        /// <summary>
        /// 验证才是是否带按钮
        /// </summary>
        protected void ValidateButton()
        {
            RuleFor(v => v.Buttons).NotEmpty().WithMessage("菜单按钮不能为空");
        }



    }
}
