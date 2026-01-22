using Blogs.AppServices.Commands.Blogs.Article;
using Blogs.AppServices.ModelValidator.Admin.Article;
using Blogs.Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Admin.Article
{
    /// <summary>
    /// 删除文章命令基类
    /// </summary>
    public class DeleteArticleCommand : ArticleCommand
    {
        public long Id { get; set; }

        public DeleteArticleCommand(long articleId)
        {
            this.Id = articleId;
        }

        public override bool IsValid()
        {
            ValidationResult = new DeleteArticleCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
