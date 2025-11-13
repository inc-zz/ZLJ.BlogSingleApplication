using Blogs.Domain.Entity.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.ResponseDto.App
{
    public class BlogsCommentDto
    {

        public long Id { get; set; }

        public long ArticleId { get; set; }

        public string ArticleTitle { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public int LikeCount { get; set; }  
        /// <summary>
        /// 回复列表
        /// </summary>
        public List<BlogsComment> Replies { get; set; }


    }
}
