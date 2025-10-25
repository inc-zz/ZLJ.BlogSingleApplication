using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Blogs.Article
{
    public class DeleteArticleCommentCommand : ArticleCommentCommand
    {
        public DeleteArticleCommentCommand(long id)
        {
            this.Id = id;
        }

        public override bool IsValid()
        {
            return this.Id > 0;
        }

    }
}
