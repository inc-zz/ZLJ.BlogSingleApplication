using Blogs.AppServices.Commands.Admin.SysButtons;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.ModelValidator.Admin.Buttons
{
    /// <summary>
    /// 创建按钮命令验证器
    /// </summary>
    public class SysButtonsCommandValidator : AbstractValidator<SysButtonsCommand>
    {
        public SysButtonsCommandValidator()
        {
           
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("按钮名称不能为空")
                .MaximumLength(50).WithMessage("按钮名称长度不能超过50个字符");

            RuleFor(x => x.Description)
                .MaximumLength(200).WithMessage("按钮描述长度不能超过200个字符");

            RuleFor(x => x.ButtonType)
                .NotEmpty().WithMessage("按钮类型不能为空")
                .Must(type => new[] { "primary", "danger", "default", "warning", "success" }.Contains(type))
                .WithMessage("按钮类型必须是 primary、danger、default、warning、success 之一");

            RuleFor(x => x.Position)
                .NotEmpty().WithMessage("按钮位置不能为空")
                .Must(position => new[] { "toolbar", "row", "both" }.Contains(position))
                .WithMessage("按钮位置必须是 toolbar、row、both 之一");

            RuleFor(x => x.SortOrder)
                .GreaterThanOrEqualTo(0).WithMessage("排序号必须大于等于0");

            RuleFor(x => x.Status)
                .InclusiveBetween(0, 1).WithMessage("状态必须是0或1");
        }
    }
}
