using Blogs.AppServices.ModelValidator.Admin.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Admin.Category
{
    public class CreateBlogsCategoryCommand : BlogsCategoryCommand
    {
        public override bool IsValid()
        {
            ValidationResult = new CreateBlogsCategoryCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
