using Blogs.AppServices.Commands.Admin.Article;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.ModelValidator.Admin.Article
{
    public abstract class BlogsCommentValidatorCommand<T> : AbstractValidator<T> where T : BlogsCommentCommand
    {
        protected void ValidateId()
        {
            RuleFor(x => x.Id).NotEqual(0).WithMessage("Id不能为空");
        }
    }
}
