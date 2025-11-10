using Blogs.AppServices.ModelValidator.Admin.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Admin.Category
{
    public class UpdateBlogsCategoryCommand : BlogsCategoryCommand
    {
        public UpdateBlogsCategoryCommand(long id)
        {
            Id = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateBlogsCategoryCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
