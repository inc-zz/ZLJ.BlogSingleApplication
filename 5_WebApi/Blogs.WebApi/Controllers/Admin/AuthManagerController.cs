using Blogs.AppServices.Commands.Admin.AuthManager;
using Blogs.AppServices.Queries.Admin;
using Blogs.AppServices.Requests.Admin;
using Blogs.Core.Models;
using Blogs.Domain.EventNotices;
using Blogs.Domain.Notices;
using Blogs.Infrastructure.Context;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Blogs.WebApi.Controllers.Admin
{
    /// <summary>
    /// 权限管理
    /// </summary>
    [ApiController]
    [Route("api/admin/[controller]")]
    public class AuthManagerController : ControllerBase
    {
        private readonly ILogger<AuthManagerController> _logger;
        private readonly IMediator _mediator;
        private readonly DomainNotificationHandler _notificationHandler;
        public AuthManagerController(ILogger<AuthManagerController> logger,
        IMediator mediator,
        INotificationHandler<DomainNotification> notifications)
        {
            _logger = logger;
            _mediator = mediator;
            _notificationHandler = notifications as DomainNotificationHandler;
        }

        /// <summary>
        /// 获取菜单按钮列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("menuActions")]
        public async Task<ActionResult> GetMenuButtonListAsync([FromQuery] GetMenuButtonListQuery request)
        {
            var result = await _mediator.Send(request);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// 角色菜单按钮授权
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("setMenuAuth")]
        public async Task<ActionResult> AuthRoleMenuAsync([FromBody] AuthRoleMenuRequest request)
        {
            var command = new AuthRoleMenuCommand(request);
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(ResultObject.Success("授权成功"));
            }
            else
            {
                var notifications = _notificationHandler.GetNotifications();
                return BadRequest(notifications);
            }
        }

        /// <summary>
        /// 角色菜单列表-左侧导航
        /// </summary>
        /// <returns></returns>
        [HttpGet("menus")]
        public async Task<ActionResult> GetRoleMenuAsync()
        {
            var roleIds = CurrentUser.Instance.UserInfo.Roles;
            var query = new GetRoleMenuAuthQuery(roleIds);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

    }
}
