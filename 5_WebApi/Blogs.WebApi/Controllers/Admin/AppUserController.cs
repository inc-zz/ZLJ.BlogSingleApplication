using Blogs.AppServices.Commands.Blogs.User;
using Blogs.AppServices.Queries.App;
using Blogs.AppServices.Requests.App;
using Blogs.Core.Models;
using Blogs.Domain.EventNotices;
using Blogs.Domain.Notices;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blogs.WebApi.Controllers.Admin
{
    /// <summary>
    /// 后台管理-App用户模块
    /// </summary>
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize]
    public class AppUserController : ControllerBase
    {
        /// <summary>
        /// 中介者-调用查询处理器
        /// </summary>
        private readonly IMediator _mediator;
        /// <summary>
        /// 领域通知处理器
        /// </summary>
        private readonly DomainNotificationHandler _notificationHandler;

        public AppUserController(IMediator mediator, INotificationHandler<DomainNotification> notifications)
        {
            _mediator = mediator;
            _notificationHandler = notifications as DomainNotificationHandler;
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<ActionResult> GetUserListAsync([FromQuery] PageParam param)
        {
            // 创建查询对象
            var query = new GetAppUserListQuery
            {
                PageIndex = param.PageIndex,
                PageSize = param.PageSize
            };

            var result = await _mediator.Send(query);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// 用户详情
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("info")]
        public async Task<ActionResult> GetUserInfoAsync([FromQuery] IdParam param)
        {
            var query = new GetAppUserInfoQuery
            {
                Id = param.Id
            };
            var result = await _mediator.Send(query);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// 启用/禁用
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("status")]
        public async Task<ActionResult> ChangeUserStatusAsync([FromBody] IdParam param)
        {
            var command = new ChangeAppUserStatusCommand(param.Id);
            var result = await _mediator.Send(command);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<ActionResult> UpdateUserAsync([FromBody] UpdateAppUserRequest param)
        {
            var command = new UpdateAppUserCommand(param);
            var result = await _mediator.Send(command);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("resetpwd")]
        public async Task<ActionResult> ResetUserPasswordAsync([FromBody] ResetAppUserPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok();
        }
    }
}
