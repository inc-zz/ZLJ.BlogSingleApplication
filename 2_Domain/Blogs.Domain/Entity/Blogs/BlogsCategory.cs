using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Domain.Entity.Blogs
{
    /// <summary>
    /// 文章分类
    /// </summary>
    [SugarTable("blogs_category")]
    public class BlogsCategory : BaseEntity
    {
        public string Name { get; private set; }
        /// <summary>
        /// 模块类型：1分类，2板块
        /// </summary>
        public string BusType { get; private set; }
        public string Description { get; private set; }
        public string? Slug { get; private set; } // URL友好名称
        public int Sort { get; private set; }

        //// 导航属性
        //private readonly List<BlogsArticle> _articles = new();

        //public virtual IReadOnlyCollection<BlogsArticle> Articles => _articles.AsReadOnly();

        public BlogsCategory() { }

        public BlogsCategory(string name, string description = null, int sort = 0)
        {
            Name = name;
            Description = description;
            Sort = sort;
        }

        public void Update(string name, string description = null, int? sort = null)
        {
            Name = name;
            Description = description;

            if (sort.HasValue)
                Sort = sort.Value;
        }

        public void Enable()
        {
            IsDeleted = 0;
        }

        public void Disable()
        {
            IsDeleted = 1;
        }
    }
}
