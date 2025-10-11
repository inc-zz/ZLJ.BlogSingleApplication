using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Blogs.Article
{
    public class UnlikeArticleCommand : ArticleCommand
    {

        public UnlikeArticleCommand()
        {
            
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
