using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Blogs.Article
{
    /// <summary>
    /// 提交评论
    /// </summary>
    public class CreateArticleCommentCommand : ArticleCommentCommand
    {

        /// <summary>
        /// 评论提交
        /// </summary>
        /// <param name="articldId"></param>
        /// <param name="comment"></param>
        public CreateArticleCommentCommand(long articldId, string comment)
        {
            this.ArticleId = articldId;
            this.Content = comment;
        }

        /// <summary>
        /// 评论回复
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="id"></param>
        /// <param name="comment"></param>
        public CreateArticleCommentCommand(long articleId, long id, string comment)
        {
            this.ArticleId = articleId;
            this.ParentId = id;
            this.Content = comment;
            this.CreatedAt = DateTime.Now;
            this.CreatedBy = CurrentAppUser.Instance.UserInfo.UserName;
        }

        override public bool IsValid()
        {
            return this.ArticleId > 0 && !string.IsNullOrEmpty(this.Content);
        }

    }
}
