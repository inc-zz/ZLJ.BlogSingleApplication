using Blogs.AppServices.ModelValidator.Admin.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Admin.Category
{
    public class DeleteBlogsCategoryCommand : BlogsCategoryCommand
    {
        public DeleteBlogsCategoryCommand(long id)
        {
            Id = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new DeleteBlogsCategoryCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
