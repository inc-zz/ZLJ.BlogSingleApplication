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
        /// <summary>
        /// 分类Id
        /// </summary>
        public long? CategoryId { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string? CategoryName { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? CreatedBy { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Summary { get;  set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get;  set; }
        /// <summary>
        /// 封面图片
        /// </summary>
        public string? CoverImage { get;  set; }
        /// <summary>
        /// 浏览量
        /// </summary>
        public int ViewCount { get;  set; }
        /// <summary>
        /// 点赞量
        /// </summary>
        public int LikeCount { get;  set; }
        /// <summary>
        /// 评论量
        /// </summary>
        public int CommentCount { get;  set; }
        /// <summary>
        /// 分享量
        /// </summary>
        public int ShareCount { get;  set; }


    }
}
