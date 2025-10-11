using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Requests.App
{
   public  class ArticleLikeRequest
    {
        public long ArticleId { get; set; }

        public long UserId { get; set; }
    }
}
