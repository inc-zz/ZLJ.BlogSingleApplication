using Blogs.Domain.Enums;
using Blogs.Core.Entity.Blogs;

namespace Blogs.Domain.Entity.Blogs
{

    /// <summary>
    /// 文章聚合根
    /// </summary>
    [SugarTable("blogs_article")]
    public class BlogsArticle : BaseEntity
    {
        // 基本属性
        public string Title { get; private set; }
        public string Summary { get; private set; }
        public string Content { get; private set; }
        public string CoverImage { get; private set; }

        // 状态属性
        public ArticleStatusEnum Status { get; private set; }
        public bool IsTop { get; private set; } // 是否置顶
        public bool IsRecommend { get; private set; } // 是否推荐

        // 统计属性
        public int ViewCount { get; private set; }
        public int LikeCount { get; private set; }
        public int CommentCount { get; private set; }
        public int ShareCount { get; private set; }

        // 时间属性
        public DateTime PublishTime { get; private set; }
        public DateTime? LastEditTime { get; private set; }

        // 外键关系
        public long AuthorId { get; private set; }
        public long? CategoryId { get; private set; }

        // 导航属性
        public virtual BlogsUser Author { get; private set; }
        public virtual BlogsCategory Category { get; private set; }
        private readonly List<BlogsTag> _articleTags = new();
        public virtual IReadOnlyCollection<BlogsTag> ArticleTags => _articleTags.AsReadOnly();
        private readonly List<BlogsComment> _comments = new();
        public virtual IReadOnlyCollection<BlogsComment> Comments => _comments.AsReadOnly();

        // 构造函数
        public BlogsArticle() { } // 为ORM保留

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="authorId"></param>
        /// <param name="summary"></param>
        /// <param name="coverImage"></param>
        public BlogsArticle(
            long id,
            string title,
            string content,
            long authorId,
            string summary = null,
            string coverImage = null)
        {
            Title = title;
            Content = content;
            AuthorId = authorId;
            Summary = summary ?? GenerateSummary(content);
            CoverImage = coverImage;
            Status = ArticleStatusEnum.Draft;
            ViewCount = 0;
            LikeCount = 0;
            CommentCount = 0;
            ShareCount = 0;
            PublishTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 根据内容生成摘要
        /// </summary>
        /// <param name="content"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        private string GenerateSummary(string content, int maxLength = 150)
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;

            // 移除HTML标签
            var plainText = System.Text.RegularExpressions.Regex.Replace(
                content, "<.*?>", string.Empty);

            // 截取指定长度
            return plainText.Length <= maxLength
                ? plainText
                : plainText.Substring(0, maxLength) + "...";
        }
    }
}
