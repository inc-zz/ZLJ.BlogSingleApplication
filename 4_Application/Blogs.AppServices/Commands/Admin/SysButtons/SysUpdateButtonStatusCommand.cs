using Blogs.Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Admin.SysButtons
{
    /// <summary>
    /// 更新按钮状态命令
    /// </summary>
    public class SysUpdateButtonStatusCommand : Command
    {
        public long Id { get; set; }
        public int Status { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new SysUpdateButtonStatusCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class SysUpdateButtonStatusCommandValidator : AbstractValidator<SysUpdateButtonStatusCommand>
    {
        public SysUpdateButtonStatusCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("按钮ID必须大于0");

            RuleFor(x => x.Status)
                .InclusiveBetween(0, 1).WithMessage("状态必须是0或1");
        }
    }
}
