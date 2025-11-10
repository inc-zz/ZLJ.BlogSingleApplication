using Blogs.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Admin.Article
{
    public abstract class BlogsCommentCommand : Command
    {
        public long Id { get; set; }
    }

}
