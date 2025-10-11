using Blogs.AppServices.Commands.Admin.SysDepartment;
using Blogs.AppServices.Commands.Admin.SysRole;
using Blogs.AppServices.Queries.Admin;
using Blogs.AppServices.Queries.ResponseDto;
using Blogs.AppServices.Requests.Admin;
using Blogs.Core.Models;
using Blogs.Domain.EventNotices;
using Blogs.Domain.Notices;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blogs.WebApi.Controllers.Admin
{
    /// <summary>
    /// 角色管理
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("/api/admin/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IMediator _mediator;
        private readonly DomainNotificationHandler _notificationHandler;

        public RoleController(ILogger<RoleController> logger,
            IMediator mediator,
            INotificationHandler<DomainNotification> notifications)
        {
            _logger = logger;
            _mediator = mediator;
            _notificationHandler = notifications as DomainNotificationHandler;
        }

        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<ActionResult> GetRoleListAsync([FromQuery] GetRoleListQuery request)
        {
            var result = await _mediator.Send(request);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// 获取所有角色（用于下拉选择）
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<ActionResult> GetAllRolesAsync()
        {
            var query = new GetAllRolesQuery();
            var result = await _mediator.Send(query);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// 获取角色详情
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("info")]
        public async Task<ActionResult> GetRoleInfoAsync([FromQuery] IdParam param)
        {
            var query = new GetRoleInfoQuery
            {
                Id = param.Id
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddRoleAsync([FromBody] AddRoleRequest request)
        {
            var command = new CreateRoleCommand(request);
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(ResultObject.Success("角色创建成功"));
            }
            else
            {
                var notifications = _notificationHandler.GetNotifications();
                return BadRequest(notifications);
            }
        }

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> EditRoleAsync([FromBody] UpdateSysRoleParam request)
        {
            var command = new UpdateRoleCommand(request);
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(ResultObject.Success("角色更新成功"));
            }
            else
            {
                var notifications = _notificationHandler.GetNotifications();
                return BadRequest(notifications);
            }
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> DeleteRoleAsync([FromBody] IdParam param)
        {
            var command = new DeleteRoleCommand(param.Id);
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(ResultObject.Success("角色删除成功"));
            }
            else
            {
                var notifications = _notificationHandler.GetNotifications();
                return BadRequest(notifications);
            }
        }

        /// <summary>
        /// 获取角色已授权的模块权限
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("module-permissions")]
        public async Task<ActionResult> GetRoleModulePermissionsAsync([FromQuery] IdParam param)
        {
            var query = new GetRoleModulePermissionsQuery
            {
                RoleId = param.Id
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// 角色模块授权
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("authorize-modules")]
        public async Task<ActionResult> AuthorizeRoleModulesAsync([FromBody] AuthRoleToMenuButtonRequest request)
        {
            var command = new AuthorizeRoleModulesCommand(request);
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(ResultObject.Success("角色模块授权成功"));
            }
            else
            {
                var notifications = _notificationHandler.GetNotifications();
                return BadRequest(notifications);
            }
        }

        /// <summary>
        /// 获取用户角色信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("user-roles")]
        public async Task<ActionResult> GetUserRolesAsync([FromQuery] IdParam param)
        {
            var query = new GetUserRolesQuery
            {
                UserId = param.Id
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// 用户角色授权
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("authorize-user")]
        public async Task<ActionResult> AuthorizeUserRolesAsync([FromBody] AuthorizeUserRolesRequest request)
        {
            var command = new AuthorizeUserRolesCommand(request);
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(ResultObject.Success("用户角色授权成功"));
            }
            else
            {
                var notifications = _notificationHandler.GetNotifications();
                return BadRequest(notifications);
            }
        }
         
    }
}
