using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Core.Entity.Blogs
{
    public class BlogsTag : BaseEntity
    {
        public string Name { get; private set; }
        public string Slug { get; private set; }
        public string Color { get; private set; } // 标签颜色
        public bool IsEnabled { get; private set; }
        public int UsageCount { get; private set; } // 使用次数

        // 导航属性
        private readonly List<BlogsTag> _articleTags = new();
        public virtual IReadOnlyCollection<BlogsTag> ArticleTags => _articleTags.AsReadOnly();

        protected BlogsTag() { }

        public BlogsTag(long id, string name, string slug, string color = "#007bff")
            : base(id)
        {
            Name = name;
            Slug = slug;
            Color = color;
            IsEnabled = true;
            UsageCount = 0;
        }

        public void Update(string name, string slug, string color = null)
        {
            Name = name;
            Slug = slug;

            if (!string.IsNullOrEmpty(color))
                Color = color;
        }

        public void IncreaseUsageCount()
        {
            UsageCount++;
        }

        public void DecreaseUsageCount()
        {
            if (UsageCount > 0)
                UsageCount--;
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
