using Blogs.AppServices.ModelValidator.Admin.Article;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Admin.Article
{
    public class DeleteBlogsCommentCommand : BlogsCommentCommand
    {
        public DeleteBlogsCommentCommand(long id)
        {
            Id = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new DeleteBlogsCommentCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
