using Blogs.AppServices.Commands.Admin.Article;
using Blogs.Domain.Entity.Blogs;
using Blogs.Domain.Notices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.CommandHandlers.Admin
{
    public class ArticleCommentCommandHandler : CommandHandler,
        IRequestHandler<DeleteBlogsCommentCommand,bool> 
    {
        public ArticleCommentCommandHandler(DomainNotificationHandler notificationHandler, ILogger<ArticleCommentCommandHandler> logger) 
            : base(notificationHandler, logger)
        {
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Handle(DeleteBlogsCommentCommand command, CancellationToken cancellationToken)
        {
            var articleComment = await DbContext.Queryable<BlogsComment>().Where(it=>it.Id == command.Id).FirstAsync();
            if(articleComment == null)
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
