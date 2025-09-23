using Blogs.Core.Entity.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Domain.Entity.Blogs
{
    /// <summary>
    /// 
    /// </summary>
    public class BlogsArticleTag : BaseEntity
    {

        public long ArticleId { get; private set; }
        public long TagId { get; private set; }

        // 导航属性
        public virtual BlogsArticle Article { get; private set; }
        public virtual BlogsTag Tag { get; private set; }

        protected BlogsArticleTag() { }

        public BlogsArticleTag(long articleId, long tagId)
        {
            ArticleId = articleId;
            TagId = tagId;
        }
    }
}
