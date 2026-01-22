using Blogs.AppServices.Commands.Admin.Article;
using Blogs.Domain.Entity.Blogs;
using Blogs.Domain.IRepositorys.Blogs;
using Blogs.Domain.Notices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.CommandHandlers.Admin
{
    /// <summary>
    /// 管理端文章操作命令处理
    /// </summary>
    public class ArticleCommandHandler : CommandHandler,
        IRequestHandler<DeleteArticleCommand, bool>
    {
        public ArticleCommandHandler(DomainNotificationHandler notificationHandler, ILogger<ArticleCommentCommandHandler> logger) : base(notificationHandler, logger)
        {

        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Handle(DeleteArticleCommand command, CancellationToken cancellationToken)
        {

            var articleComment = await DbContext.Queryable<BlogsArticle>().Where(it => it.Id == command.Id).FirstAsync();
            if (articleComment == null)
            {
                return false;
            }
            articleComment.IsDeleted = 1;
            articleComment.MarkAsModified(CurrentUser.Instance.UserInfo.UserName);
            var result = await DbContext.Updateable(articleComment).ExecuteCommandAsync();
            return result > 0;

        }
    }
}
