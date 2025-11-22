using Blogs.Core.Entity.Blogs;
using Blogs.Domain.Enums;

namespace Blogs.Domain.Entity.Blogs
{
    /// <summary>
    /// 博客评论表
    /// </summary>
    [SugarTable("website_suggest")]
    public class WebSiteSuggest : BaseEntity
    {

        public string? UserName { get; set; }

        public string? Contact { get; set; }

        public string? Content { get; set; }

    }
}
