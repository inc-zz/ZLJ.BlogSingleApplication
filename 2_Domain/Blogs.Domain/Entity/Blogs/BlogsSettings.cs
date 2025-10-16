using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Domain.Entity.Blogs
{
    /// <summary>
    /// 博客设置
    /// </summary>
    [SugarTable("blogs_settings")]
    public class BlogsSettings : BaseEntity
    {

        public string? Title { get; private set; }
        public string? Summary { get; private set; }
        public string? Url { get; private set; }
        public string? Tags { get; private set; }
        public string? BusType { get; private set; }
        public string? Content { get; private set; }
        public int Status { get; private set; }

        public void SetEntity(string title, string summary, string url, string tags, string busType, string content, int status)
        {
            this.Title = title;
            this.Summary = summary;
            this.Url = url;
            this.Tags = tags;
            this.BusType = busType;
            this.Content = content;
            this.Status = status;
        }

    }
}
