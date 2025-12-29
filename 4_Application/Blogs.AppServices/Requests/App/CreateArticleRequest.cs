using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Requests.App
{
    /// <summary>
    /// 发布文章
    /// </summary>
    public class CreateArticleRequest
    {
        public long? Id { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public long? CategoryId { get; set; }

        public string? CoverImage { get; set; }

        public string? Tags { get; set; }

        public string? Content { get; set; }

        public bool IsPublish { get; set; }

        private string? CreateBy;
        public void SetCreateBy(string userName)
        {
            this.CreateBy = userName;
        }
        public string GetCreateBy()
        {
            return this.CreateBy;
        }


    }
}
