using Blogs.Domain;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Blogs.Article
{
    public abstract class ArticleCommand : Command
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 文章标题
        /// </summary>
        public string? Title { get; private set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Summary { get; private set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; private set; }
        /// <summary>
        /// 封面图片
        /// </summary>
        public string? CoverImage { get; private set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; private set; }
        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool IsTop { get; private set; }
        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool IsRecommend { get; private set; }

        /// <summary>
        /// 浏览量
        /// </summary>
        public int ViewCount { get; private set; }
        /// <summary>
        /// 点赞量
        /// </summary>
        public int LikeCount { get; private set; }
        /// <summary>
        /// 评论量
        /// </summary>
        public int CommentCount { get; private set; }
        /// <summary>
        /// 分享量
        /// </summary>
        public int ShareCount { get; private set; }

        // 外键关系
        public long AuthorId { get; private set; }
        /// <summary>
        /// 分类ID
        /// </summary>
        public long? CategoryId { get; private set; }
    }
}
