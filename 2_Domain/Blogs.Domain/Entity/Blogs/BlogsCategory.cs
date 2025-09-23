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
    public class BlogsCategory : BaseEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Slug { get; private set; } // URL友好名称
        public int DisplayOrder { get; private set; }
        public bool IsEnabled { get; private set; }

        // 导航属性
        private readonly List<BlogsArticle> _articles = new();
        public virtual IReadOnlyCollection<BlogsArticle> Articles => _articles.AsReadOnly();

        protected BlogsCategory() { }

        public BlogsCategory(long id, string name, string slug, string description = null,
            int displayOrder = 0)
        {
            Name = name;
            Slug = slug;
            Description = description;
            DisplayOrder = displayOrder;
            IsEnabled = true;
        }

        public void Update(string name, string slug, string description = null, int? displayOrder = null)
        {
            Name = name;
            Slug = slug;
            Description = description;

            if (displayOrder.HasValue)
                DisplayOrder = displayOrder.Value;
        }

        public void Enable()
        {
            IsEnabled = true;
        }

        public void Disable()
        {
            IsEnabled = false;
        }
    }
}
