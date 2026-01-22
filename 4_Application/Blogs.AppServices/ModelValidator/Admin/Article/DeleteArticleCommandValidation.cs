using Blogs.AppServices.Commands.Admin.Article;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.ModelValidator.Admin.Article
{
    public class DeleteArticleCommandValidation: BlogsArticleValidatorCommand<DeleteArticleCommand>
    {

        public DeleteArticleCommandValidation()
        {
            ValidateId();
        }
    }
}
