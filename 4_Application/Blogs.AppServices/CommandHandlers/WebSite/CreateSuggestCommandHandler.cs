using Blogs.AppServices.CommandHandlers.Admin;
using Blogs.AppServices.Commands.WebSite;
using Blogs.Domain.Entity.Blogs;
using Blogs.Domain.Notices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.CommandHandlers.WebSite
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateSuggestCommandHandler : CommandHandler,
         IRequestHandler<CreateSuggestCommand, bool>
    {

        private readonly IMediatorHandler _eventBus;
        private readonly IHttpContextAccessor _httpContext;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventBus"></param>
        /// <param name="httpContext"></param>
        /// <param name="logger"></param>
        /// <param name="notificationHandler"></param>
        public CreateSuggestCommandHandler(IMediatorHandler eventBus,
        IHttpContextAccessor httpContext,
        ILogger<CreateSuggestCommandHandler> logger,
        DomainNotificationHandler notificationHandler) : base(notificationHandler, logger)
        {
            _eventBus = eventBus;
            _httpContext = httpContext;
        }

        /// <summary>
        /// 创建建议执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Handle(CreateSuggestCommand request, CancellationToken cancellationToken)
        {
            var comment = request.Adapt<WebSiteSuggest>();
            comment.CreatedAt = DateTime.Now;
            comment.CreatedBy = IPAddress.Loopback.ToString();

            var result = await DbContext.Insertable(comment).ExecuteCommandAsync();
            return result > 0;
        }
    }
}
