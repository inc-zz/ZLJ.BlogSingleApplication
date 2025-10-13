using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Blogs.Article
{
    public class CreateArticleCommentCommand : ArticleCommand
    {

        public CreateArticleCommentCommand(long articleId, string comment)
        {
            this.Id = articleId;

        }

        override public bool IsValid()
        {
            return  this.Id>0 && !string.IsNullOrEmpty(this.Content);
        }

    }
}
