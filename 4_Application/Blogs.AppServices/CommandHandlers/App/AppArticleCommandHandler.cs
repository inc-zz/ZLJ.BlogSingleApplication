using Blogs.AppServices.Commands.Blogs.Article;
using Blogs.Domain.Entity.Blogs;
using Blogs.Domain.IRepositorys.Blogs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.CommandHandlers.App
{
    public class AppArticleCommandHandler : AppCommandHandler,
        IRequestHandler<CreateArticleCommand, bool>,
        IRequestHandler<LikeArticleCommand, bool>,
        IRequestHandler<CreateArticleCommentCommand, bool>,
        IRequestHandler<DeleteArticleCommentCommand,bool>
    {
        private readonly IMediatorHandler _eventBus;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IAppArticleRepository _appArticleRepository;
        public AppArticleCommandHandler(IMediatorHandler eventBus,
             IHttpContextAccessor httpContext,
             IAppArticleRepository appArticleRepository)
             : base(eventBus)
        {
            _eventBus = eventBus;
            _httpContext = httpContext;
            _appArticleRepository = appArticleRepository;
        }

        /// <summary>
        /// 发布文章
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(CreateArticleCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
                return await Task.FromResult(false);
            }
            try
            {
                var article = command.Adapt<BlogsArticle>();
                if (command.Id > 0)
                {
                    article.ModifiedBy = command.CreateBy;
                    await _appArticleRepository.UpdateArticleAsync(article);
                }
                else
                {
                    article.Id = new NCD.Common.IdWorkerUtils().NextId();
                    //创建人信息
                    article.CreatedAt = DateTime.Now;
                    article.MarkAsCreated(command.CreateBy);
                    await _appArticleRepository.InsertArticleAsync(article);
                }
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _eventBus.RaiseEvent(new DomainNotification("AppArticleCommandHandler", ex.Message));
                return await Task.FromResult(false);
            }
        }

        /// <summary>
        /// 文章点赞
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Handle(LikeArticleCommand command, CancellationToken cancellationToken)
        {

            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
                return await Task.FromResult(false);
            }
            try
            {
                var article = await DbContext.Queryable<BlogsArticle>().Where(it => it.Id == command.Id).FirstAsync();
                if (article == null)
                {
                    return await Task.FromResult(false);
                }
                //点赞记录：

                //点赞累计 
                article.SetlikeCount(command.LikeCount);
                article.SetModifyInfo(command.UserName);
                var result = await DbContext.Updateable(article).ExecuteCommandAsync();
                return result > 0;

            }
            catch (Exception e)
            {
                throw;
            }

        }

        /// <summary>
        /// 提交评论
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Handle(CreateArticleCommentCommand command, CancellationToken cancellationToken)
        {
            var articleInfo = await DbContext.Queryable<BlogsArticle>()
                .Where(it => it.Id == command.ArticleId).FirstAsync();

            if (articleInfo == null)
            {
                throw new Exception("文章不存在，无法评论！");
            }
            if (!command.IsValid())
            {
                return await Task.FromResult(false);
            }
            try
            {

                var comment = new BlogsComment();
                var userName = CurrentAppUser.Instance.UserInfo.UserName;
                if (command.ParentId == 0)
                    comment.SetComment(command.ArticleId, command.Content, userName);
                else
                    comment.ReplyComment(command.ArticleId, command.ParentId, command.Content, userName);
                var result = await DbContext.Insertable(comment).ExecuteCommandAsync();
                return result > 0;

            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Handle(DeleteArticleCommentCommand request, CancellationToken cancellationToken)
        {
            var userName = CurrentAppUser.Instance.UserInfo.UserName;
            var comment = await DbContext.Queryable<BlogsComment>()
                .Where(it => it.Id == request.Id && it.CreatedBy == userName).FirstAsync();
            if(comment == null)
            {
                return false;
            }
            var result = await DbContext.Updateable<BlogsComment>().SetColumns(it => new BlogsComment { IsDeleted  = 1 })
                .WhereColumns(it => it.Id == comment.Id || it.ParentId == comment.Id).ExecuteCommandAsync();

            return result > 0;
        }
    }
}
