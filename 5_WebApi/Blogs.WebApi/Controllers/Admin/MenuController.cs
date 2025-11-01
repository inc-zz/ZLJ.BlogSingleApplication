using Blogs.AppServices.Commands.Admin.SysMenu;
using Blogs.AppServices.Commands.Admin.SysRole;
using Blogs.AppServices.Queries.Admin;
using Blogs.AppServices.Requests.Admin;
using Blogs.Core.Models;
using Blogs.Domain.Entity.Admin;
using Blogs.Domain.EventNotices;
using Blogs.Domain.Notices;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blogs.WebApi.Controllers.Admin
{
    /// <summary>
    /// 菜单管理控制器
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/admin/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly DomainNotificationHandler _notificationHandler;
        public MenuController(IMediator mediator,
            INotificationHandler<DomainNotification> notifications)
        {
            _mediator = mediator;
            _notificationHandler = notifications as DomainNotificationHandler;
        }

        /// <summary>
        /// 菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<ActionResult> GetMenuListAsync([FromQuery] GetMenuListQuery request)
        {
            var result = await _mediator.Send(request);
            return new OkObjectResult(result);
        }
        /// <summary>
        /// 获取菜单树形结构
        /// </summary>
        /// <returns></returns>
        [HttpGet("tree")]
        public async Task<ActionResult> GetTreeMenuAsync([FromQuery] GetMenuTreeQuery request)
        {
            var result = await _mediator.Send(request);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CreateMenuAsync([FromBody] AddMenuRequest request)
        {
            var command = new CreateMenuCommand(request);
            var result = await _mediator.Send(command);
            if (result)
            {
                 
                return Ok(ResultObject.Success("菜单创建成功"));
            }
            else
            {
                var notifications = _notificationHandler.GetNotifications();
                return BadRequest(notifications);
            }
        }

        /// <summary>
        /// 菜单详情
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("info")]
        public async Task<ActionResult> GetSysMenuAsync([FromQuery] IdParam param)
        {
            var query = new GetMenuInfoQuery
            {
                Id = param.Id
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }


        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> DeleteMenuAsync([FromBody] IdParam request)
        {
            var command = new DeleteMenuCommand(request.Id);
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(ResultObject.Success("菜单删除成功"));
            }
            else
            {
                var notifications = _notificationHandler.GetNotifications();
                return BadRequest(notifications);
            }
        }
        /// <summary>
        /// 编辑菜单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> UpdateMenuAsync([FromBody] UpdateMenuRequest request)
        {
            var command = new UpdateMenuCommand(request);
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(ResultObject.Success("菜单修改成功"));
            }
            else
            {
                var notifications = _notificationHandler.GetNotifications();
                return BadRequest(notifications);
            }
        }
    }
}
