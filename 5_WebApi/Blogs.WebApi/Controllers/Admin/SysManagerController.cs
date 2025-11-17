using Blogs.AppServices.Queries.Admin;
using Blogs.AppServices.Requests.App;
using Blogs.Core.Models;
using Blogs.Domain.EventNotices;
using Blogs.Domain.Notices;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blogs.WebApi.Controllers.Admin
{
    /// <summary>
    /// 系统管理
    /// </summary>
    [ApiController]
    [Route("/api/admin/[controller]")]
    public class SysManagerController : ControllerBase
    {
        private readonly ILogger<SysManagerController> _logger;
        private readonly IMediator _mediator;
        private readonly DomainNotificationHandler _notificationHandler;
        public SysManagerController(ILogger<SysManagerController> logger, 
            IMediator mediator,
            INotificationHandler<DomainNotification> notifications)
        {
            _logger = logger;
            _mediator = mediator;
            _notificationHandler = notifications as DomainNotificationHandler;
        }

        /// <summary>
        /// 配置列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("config/list")]
        public async Task<IActionResult> GetConfigList(GetBlogsConfigQuery request)
        {
            var result = await _mediator.Send(request);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// 添加/修改配置
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("config/set")]
        public async Task<IActionResult> SetConfigAsync([FromBody] SetBlogsConfigRequest request)
        {
            var result = await _mediator.Send(request);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// 删除配置
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteConfigAsync([FromBody] IdParam request)
        {
            var result = await _mediator.Send(request);
            return new OkObjectResult(result);
        }

    }
}
