using Blogs.AppServices.ModelValidator.Admin.Buttons;
using Blogs.AppServices.Requests.Admin;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Admin.SysButtons
{
    /// <summary>
    /// 更新按钮命令
    /// </summary>
    public class SysUpdateButtonCommand : SysButtonsCommand
    {

        public SysUpdateButtonCommand(UpdateSysButtonRequest request)
        {
            this.Id = request.Id;
            this.Name = request.Name;
            this.Code = request.Code;
            this.Description = request.Description;
            this.Icon = request.Icon;
        }

        public override bool IsValid()
        {
            ValidationResult = new SysUpdateButtonCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SysUpdateButtonCommandValidator : SysButtonsCommandValidator
    {
        public SysUpdateButtonCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("按钮ID必须大于0");
        }
    }
}
