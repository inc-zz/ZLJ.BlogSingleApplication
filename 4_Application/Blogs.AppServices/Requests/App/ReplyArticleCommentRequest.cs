using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Requests.App
{
    public class ReplyArticleCommentRequest
    {

        public long ArticleId {  get; set; }
        public long ParentId { get; set; }

        public string Content { get; set; }
    }
}
