using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Requests.App
{
    /// <summary>
    /// 文章列表请求参数
    /// </summary>
    public class ArticleListRequest : PageParam
    {
        public int? CategoryId { get; set; }
        public int? TagId { get; set; }
        public string? Keyword { get; set; }
        public string SortBy { get; set; } = "PublishDate";
    }
}
