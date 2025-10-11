using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Blogs.Article
{
    public class LikeArticleCommand : ArticleCommand
    {
        public LikeArticleCommand(long id)
        {
            this.Id = id;
        }

        public override bool IsValid()
        {
            return this.Id > 0;
        }
    }
}
