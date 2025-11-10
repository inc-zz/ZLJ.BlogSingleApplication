using Blogs.Core.Entity.Blogs;
using Blogs.Domain.Enums;

namespace Blogs.Domain.Entity.Blogs
{
    /// <summary>
    /// 博客评论表
    /// </summary>
    [SugarTable("blogs_comment")]
    public class BlogsComment : BaseEntity
    {

        public long ParentId { get; private set; }
        public long ArticleId { get; private set; }

        public long AuthorId { get; private set; }

        public long ReplyToUserId { get; private set; }

        public string Content { get; private set; }
        public int Status { get; private set; }
        public int LikeCount { get; private set; }

        public BlogsComment()
        {

        }
        /// <summary>
        /// 评论文章
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="content"></param>
        public void SetComment(long articleId, string content, string userName)
        {
            this.ArticleId = articleId;
            this.Content = content;
            this.CreatedBy = userName;
            this.CreatedAt    = DateTime.Now;
            this.ParentId = 0;
        }

        /// <summary>
        /// 回复评论
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="pid"></param>
        /// <param name="content"></param>
        /// <param name="userName"></param>
        public void ReplyComment(long articleId, long pid, string content, string userName)
        {
            this.ArticleId = articleId;
            this.ParentId = pid;
            this.Content = content;
            this.CreatedAt = DateTime.Now;
            this.CreatedBy = userName;
        }

        public void LikeArticle()
        {
            this.LikeCount += 1;
        }
        public void DislikeArticle()
        {
            if (this.LikeCount > 0)
            {
                this.LikeCount -= 1;
            }
        }


    }
}
