using Blogs.AppServices.Commands.Admin.Category;
using Blogs.AppServices.Commands.Admin.SysConfig;
using Blogs.AppServices.Commands.Blogs.Settings;
using Blogs.AppServices.Queries.Admin;
using Blogs.AppServices.Requests.Admin;
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
    [Route("api/admin/[controller]")]
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
        [HttpGet("list")]
        public async Task<IActionResult> GetConfigList([FromQuery] GetBlogsConfigQuery request)
        {
            var result = await _mediator.Send(request);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// 添加/修改配置
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("set")]
        public async Task<IActionResult> SetConfigAsync([FromBody] SetBlogsConfigRequest request)
        {
            if (request.Id > 0)
            {
                var command = new UpdateBlogSettingCommand(request);
                var result = await _mediator.Send(command);
                return Ok(ResultObject.Success("处理成功"));
            }
            else
            {
                var command = new CreateBlogSettingsCommand(request);
                var result = await _mediator.Send(command);
                return Ok(ResultObject.Success("处理成功"));
            }
        }

        /// <summary>
        /// 删除配置
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteConfigAsync([FromBody] IdParam request)
        {
            var command = new DeleteBlogSettingsCommand(request.Id);
            var isDeleted = await _mediator.Send(command);
            if (isDeleted)
                return Ok(ResultObject.Success("删除成功"));
            return Ok(ResultObject.Success("删除失败"));
        }

    }
}
