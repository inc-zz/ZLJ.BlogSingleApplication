using Blogs.AppServices.Commands.Admin.Category;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.ModelValidator.Admin.Category
{
    public abstract class BlogsCategoryValidatorCommand<T> : AbstractValidator<T> where T : BlogsCategoryCommand
    {
        protected void ValidateName()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("分类名称不能为空")
                .MaximumLength(50).WithMessage("分类名称长度不能超过50个字符");
        }

        protected void ValidateDescription()
        {
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("分类描述长度不能超过500个字符");
        }

        protected void ValidateId()
        {
            RuleFor(x => x.Id).NotEqual(0).WithMessage("Id不能为空");
        }
    }
}
