using Blogs.Domain.EventNotices;
using Blogs.Domain.Notices;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blogs.WebApi.Controllers.App
{
    /// <summary>
    /// 前端-开放API模块
    /// </summary>
    [Route("api/app/[controller]")]
    [ApiController]
    public class OpenApiController : ControllerBase
    {

        private readonly ILogger<OpenApiController> _logger;
        private readonly IMediator _mediator; //查询调用处理器
        private readonly DomainNotificationHandler _notificationHandler; //领域通知处理器
        /// <summary>
        /// 
        /// </summary>
        public OpenApiController(ILogger<OpenApiController> logger,
            IMediator mediator,
            INotificationHandler<DomainNotification> notifications)
        {
            _logger = logger;
            _mediator = mediator;
            _notificationHandler = notifications as DomainNotificationHandler;
        }






    }
}
