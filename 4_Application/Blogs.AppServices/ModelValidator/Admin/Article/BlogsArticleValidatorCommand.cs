using Blogs.AppServices.Commands.Admin.Article;
using Blogs.AppServices.Commands.Blogs.Article;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.ModelValidator.Admin.Article
{
    public class BlogsArticleValidatorCommand<T> : AbstractValidator<T> where T : ArticleCommand
    {
        protected void ValidateId()
        {
            RuleFor(x => x.Id).NotEqual(0).WithMessage("Id不能为空");
        }
    }
}
