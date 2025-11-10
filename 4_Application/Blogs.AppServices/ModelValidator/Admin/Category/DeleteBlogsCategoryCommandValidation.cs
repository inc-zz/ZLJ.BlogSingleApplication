using Blogs.AppServices.Commands.Admin.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.ModelValidator.Admin.Category
{
    public class DeleteBlogsCategoryCommandValidation : BlogsCategoryValidatorCommand<DeleteBlogsCategoryCommand>
    {
        public DeleteBlogsCategoryCommandValidation()
        {
            ValidateId();
        }
    }
}
