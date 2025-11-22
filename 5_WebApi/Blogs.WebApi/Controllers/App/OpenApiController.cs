using Blogs.AppServices.Commands.Blogs.Settings;
using Blogs.AppServices.Commands.WebSite;
using Blogs.AppServices.Requests.App;
using Blogs.Core.Models;
using Blogs.Domain.EventNotices;
using Blogs.Domain.Notices;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// 提交建议
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("suggest")]
        [AllowAnonymous]
        public async Task<ActionResult> SaveSuggestAsync([FromBody] AppSuggestRequest request)
        {
            var command = new CreateSuggestCommand(request);
            // 发送命令并获取结果
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(ResultObject.Success("创建成功！"));
            }
            return BadRequest(ResultObject.Success("创建失败！"));
        }

    }
}
