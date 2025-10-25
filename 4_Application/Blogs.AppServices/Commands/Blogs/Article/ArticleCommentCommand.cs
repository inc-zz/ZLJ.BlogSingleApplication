using Blogs.Domain;
using Blogs.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Blogs.Article
{
    /// <summary>
    /// 文章评论命令基类
    /// </summary>
    public abstract class ArticleCommentCommand : Command
    {

        /// <summary>
        /// 主键
        /// </summary>
        public  long Id { get; set; }

        public long ParentId { get;  set; }
        public long ArticleId { get;  set; }

        public long AuthorId { get;  set; }

        public long ReplyToUserId { get;  set; }

        public string? Content { get;  set; }
        public int Status { get;  set; }
        public int LikeCount { get;  set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string? CreatedBy { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? ModifiedAt { get; set; }
        /// <summary>
        /// 最后修改人
        /// </summary>
        public string? ModifiedBy { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDeleted { get;  set; } = 0;
 
    }
}
