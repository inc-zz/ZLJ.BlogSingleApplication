using Blogs.Core.Entity.Blogs;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Core.Entity.Blogs
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("blogs_article_tag")]
    public class DbBlogsArticleTag : BaseEntity
    {
        public long ArticleId { get; private set; }
        public long TagId { get; private set; }
         
    }
}
