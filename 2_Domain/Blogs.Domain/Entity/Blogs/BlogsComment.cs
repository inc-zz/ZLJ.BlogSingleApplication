using Blogs.Core.Entity.Blogs;
using Blogs.Domain.Enums;

namespace Blogs.Domain.Entity.Blogs
{
    /// <summary>
    /// 
    /// </summary>
    public class BlogsComment : BaseEntity
    {
        public string Content { get; private set; }
        public CommentStatusEnum Status { get; private set; }
        public int LikeCount { get; private set; }
        public DateTime CreateTime { get; private set; }

        // 外键关系
        public long ArticleId { get; private set; }
        public long AuthorId { get; private set; }
        public long? ParentId { get; private set; } // 父评论ID（支持回复）
        public long? ReplyToUserId { get; private set; } // 回复给谁

        // 导航属性
        public virtual BlogsArticle Article { get; private set; }
        public virtual BlogsUser Author { get; private set; }
        public virtual BlogsComment Parent { get; private set; }
        public virtual BlogsUser ReplyToUser { get; private set; }
        private readonly List<BlogsComment> _replies = new();
        public virtual IReadOnlyCollection<BlogsComment> Replies => _replies.AsReadOnly();

        protected BlogsComment() { }

        public BlogsComment(
            long id,
            string content,
            long articleId,
            long authorId,
            long? parentId = null,
            long? replyToUserId = null)
            : base(id)
        {
            Content = content;
            ArticleId = articleId;
            AuthorId = authorId;
            ParentId = parentId;
            ReplyToUserId = replyToUserId;
            Status = CommentStatusEnum.Pending; // 默认待审核
            LikeCount = 0;
            CreateTime = DateTime.UtcNow;

            //AddDomainEvent(new CommentCreatedEvent(Id, ArticleId, AuthorId));
        }

        public void Approve()
        {
            Status = CommentStatusEnum.Approved;

            //AddDomainEvent(new CommentApprovedEvent(Id));
        }

        public void Reject()
        {
            Status = CommentStatusEnum.Rejected;

            //AddDomainEvent(new CommentRejectedEvent(Id));
        }

        public void IncreaseLikeCount()
        {
            LikeCount++;

            //AddDomainEvent(new CommentLikedEvent(Id, LikeCount));
        }

        public void UpdateContent(string content)
        {
            Content = content;

            //AddDomainEvent(new CommentUpdatedEvent(Id));
        }
    }
}
