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
    /// 删除按钮命令
    /// </summary>
    public class SysDeleteButtonCommand : Command
    {
        public long Id { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new SysDeleteButtonCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class SysDeleteButtonCommandValidator : AbstractValidator<SysDeleteButtonCommand>
    {
        public SysDeleteButtonCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("按钮ID必须大于0");
        }
    }
}
