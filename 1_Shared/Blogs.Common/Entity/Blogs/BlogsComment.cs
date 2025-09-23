using Blogs.Core.Entity;
using Blogs.Core.Entity.Blogs;
using SqlSugar;

namespace Blogs.Core.Entity.Blogs
{
    /// <summary>
    /// Blogs评论
    /// </summary>
    [SugarTable("blogs_comment")]
    public class DbBlogsComment : BaseEntity
    {
        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; private set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; private set; }
        /// <summary>
        /// 点赞数
        /// </summary>
        public int LikeCount { get; private set; }

        /// <summary>
        /// 评论所属文章ID
        /// </summary>
        public long ArticleId { get; private set; }
        /// <summary>
        /// 评论作者ID
        /// </summary>
        public long AuthorId { get; private set; }
        /// <summary>
        /// 父级评论ID
        /// </summary>
        public long? ParentId { get; private set; }
        /// <summary>
        /// 回复给谁
        /// </summary>
        public long? ReplyToUserId { get; private set; } 
       
    }
}
