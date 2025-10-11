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
    [SugarTable("blogs_article_tag")]
    public class BlogsArticleTag : BaseEntity
    {

        public long Id { get; private set; }

        public string Name { get; set; }

        public int Sort { get; private set; }

        protected BlogsArticleTag() { }

     
    }
}
