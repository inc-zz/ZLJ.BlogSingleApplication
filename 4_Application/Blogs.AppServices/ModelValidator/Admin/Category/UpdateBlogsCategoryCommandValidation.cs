using Blogs.AppServices.Commands.Admin.Category;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.ModelValidator.Admin.Category
{
    public class UpdateBlogsCategoryCommandValidation : BlogsCategoryValidatorCommand<UpdateBlogsCategoryCommand>
    {
        public UpdateBlogsCategoryCommandValidation()
        {
            ValidateId();
            ValidateName();
            ValidateDescription();
            RuleFor(x => x.Sort).GreaterThanOrEqualTo(0).WithMessage("排序值必须大于等于0");
        }
    }
}
