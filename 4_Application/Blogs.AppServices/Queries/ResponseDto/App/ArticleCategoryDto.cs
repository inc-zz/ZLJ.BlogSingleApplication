using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.ResponseDto.App
{
    public class ArticleCategoryDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 文章数量
        /// </summary>
        public int ArticleCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Slug { get; set; }

    }
}
