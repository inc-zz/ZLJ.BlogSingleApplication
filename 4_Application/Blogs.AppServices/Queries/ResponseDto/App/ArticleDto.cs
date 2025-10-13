using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.ResponseDto.App
{
    public class ArticleDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 文章标题
        /// </summary>
        public string? Title { get;  set; } 

        public DateTime? CreatedAt { get; set; }

        public string? CreatedBy { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Summary { get;  set; } 
        /// <summary>
        /// 封面图片
        /// </summary>
        public string? CoverImage { get;  set; }
         
        public string? CategoryName { get; set; }

        public int ViewCount { get; set; }

        public int LikeCount { get; set; }
    }
}
