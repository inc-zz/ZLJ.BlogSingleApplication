using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Requests.App
{
    public class GetArticleCommentsRequest : PageParam
    {
        public long ArticleId { get; set; }

    }
}
