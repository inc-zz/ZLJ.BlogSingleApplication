using Blogs.AppServices.Commands.Admin.Category;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.ModelValidator.Admin.Category
{

    public class CreateBlogsCategoryCommandValidation : BlogsCategoryValidatorCommand<CreateBlogsCategoryCommand>
    {
        public CreateBlogsCategoryCommandValidation()
        {
            ValidateName();
            ValidateDescription();
            RuleFor(x => x.Sort).GreaterThanOrEqualTo(0).WithMessage("排序值必须大于等于0");
        }
    }
}
