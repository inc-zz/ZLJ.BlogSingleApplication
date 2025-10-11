using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Requests.App
{
    public class ArticleSearchRequest : PageParam
    {
        public string Keyword { get; set; }
        public int? CategoryId { get; set; }
        public int? TagId { get; set; }
    }
}
