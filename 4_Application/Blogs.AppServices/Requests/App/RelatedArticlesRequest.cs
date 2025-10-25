using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Requests.App
{
    /// <summary>
    /// 
    /// </summary>
    public class RelatedArticlesRequest
    {
        public long ArticleId { get; set; }

        public int TopCount { get; set; }

    }
}
