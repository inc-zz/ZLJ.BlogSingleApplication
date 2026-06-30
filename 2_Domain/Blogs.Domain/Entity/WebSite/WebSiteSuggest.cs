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
        /// <summary>
        /// 用户
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string? Contact { get; set; }
        /// <summary>
        /// 建议内容
        /// </summary>
        public string? Content { get; set; }

    }
}
