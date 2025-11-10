using Blogs.Domain;
using Blogs.Domain.Entity.Blogs;
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

        public string? Tags { get; private set; }
        /// <summary>
        /// 操作用户
        /// </summary>

        public string? UserName { get; private set; }


        public string? Comment { get; set; }

        /// <summary>
        /// 设置文章信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="summary"></param>
        /// <param name="content"></param>
        /// <param name="coverImage"></param>
        /// <param name="status"></param>
        /// <param name="isTop"></param>
        /// <param name="isRecommend"></param>
        /// <param name="categoryId"></param>
        public void SetArticleInfo(long? id, string title, string summary, string content, string tags, long? categoryId, string? coverImage,string? createUser)
        {
            if (id.HasValue)
            {
                this.Id = id.Value;
            }
            this.Title = title;
            this.Summary = summary;
            this.Content = content;
            this.Tags = tags;
            this.CoverImage = coverImage;
            this.Status = 1;
            this.IsTop = true;
            this.IsRecommend = false;
            this.CategoryId = categoryId;
            this.CreateBy = createUser;
        }

        /// <summary>
        /// 点赞/取消点赞
        /// </summary>
        /// <param name="val"></param>
        public void SetLikeCount(long id, int val, string userName)
        {
            this.Id = id;
            this.LikeCount += val;
            this.UserName = userName;
        }


    }
}
