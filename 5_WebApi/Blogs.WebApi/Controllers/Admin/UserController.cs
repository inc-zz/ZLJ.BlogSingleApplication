using Blogs.AppServices.Commands.Admin.SysUser;
using Blogs.AppServices.Queries.Admin;
using Blogs.AppServices.Requests.Admin;
using Blogs.Core.Models;
using Blogs.Domain.EventNotices;
using Blogs.Domain.Notices;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Blogs.WebApi.Controllers.Admin
{

    /// <summary>
    /// 管理员用户
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/admin/[controller]")]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// 中介者-调用查询处理器
        /// </summary>
        private readonly IMediator _mediator;
        /// <summary>
        /// 领域通知处理器
        /// </summary>
        private readonly DomainNotificationHandler _notificationHandler;

        /// <summary>
        /// 
        /// </summary>
        public UserController(IMediator mediator, INotificationHandler<DomainNotification> notifications)
        {
            _mediator = mediator;
            _notificationHandler = notifications as DomainNotificationHandler;
        }

        /// <summary>
        /// 管理员列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<ActionResult> GetAdminUserList([FromQuery] PageParam param)
        {
            // 创建查询对象
            var query = new GetUserListQuery
            {
                PageIndex = param.PageIndex,
                PageSize = param.PageSize 
            };

            // 通过中介者发送查询请求
            var result = await _mediator.Send(query);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// 获取管理员用户信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("info")]
        public async Task<ActionResult> GetAdminInfoAsync([FromQuery] IdParam param)
        {
            var query = new GetUserInfoQuery
            {
                Id = param.Id
            };
            // 通过中介者发送查询请求
            var result = await _mediator.Send(query);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// 添加管理员用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddAdminUserAsync([FromBody] AddUserRequest request)
        {
            var command = new CreateUserCommand(request);

            var result = await _mediator.Send(command);

            if (result)
            {
                return Ok(ResultObject.Success("处理成功"));
            }
            else
            {
                //处理失败，返回错误信息
                var notifications = _notificationHandler.GetNotifications();
                return BadRequest(notifications);
            }
        }

        /// <summary>
        /// 编辑管理员用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> EditAdminUserAsync([FromBody] UpdateUserRequest request)
        {
            var command = new UpdateUserCommand(request);
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(ResultObject.Success("处理成功"));
            }
            else
            {
                //处理失败，返回错误信息
                var notifications = _notificationHandler.GetNotifications();
                return BadRequest(notifications);
            }
        }


        /// <summary>
        /// 删除管理员用户
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> DeleteAdminUserAsync([FromBody] IdParam param)
        {
            var command = new DeleteUserCommand(param.Id);
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(ResultObject.Success("删除成功"));
            }
            else
            {
                var notifications = _notificationHandler.GetNotifications();
                return BadRequest(notifications);
            }
        }

        /// <summary>
        /// 设置用户状态（启用/禁用）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("status")]
        public async Task<ActionResult> SetUserStatusAsync([FromBody] ChangeStatusRequest request)
        {
            var command = new ChangeUserStatusCommand(request);
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(ResultObject.Success("状态更新成功"));
            }
            else
            {
                var notifications = _notificationHandler.GetNotifications();
                return BadRequest(notifications);
            }
        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("reset-password")]
        public async Task<ActionResult> ResetUserPasswordAsync([FromBody] ChangePasswordRequest request)
        {
            var command = new ResetPasswordCommand(request);
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(ResultObject.Success("密码重置成功"));
            }
            else
            {
                var notifications = _notificationHandler.GetNotifications();
                return BadRequest(notifications);
            }
        } 

        /// <summary>
        /// 批量设置用户角色
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("set-roles")]
        public async Task<ActionResult> SetUserRolesAsync([FromBody] SetUserRolesRequest request)
        {
            var command = new UserRoleAuthCommand(request);
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(ResultObject.Success("角色设置成功"));
            }
            else
            {
                var notifications = _notificationHandler.GetNotifications();
                return BadRequest(notifications);
            }
        }
    }
}
