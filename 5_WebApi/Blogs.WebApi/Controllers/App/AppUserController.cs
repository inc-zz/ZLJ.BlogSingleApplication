using Azure.Core;
using Blogs.AppServices.Commands.Admin.SysUser;
using Blogs.AppServices.Commands.Blogs.User;
using Blogs.AppServices.Requests.Admin;
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
    /// 前端-用户模块
    /// </summary>
    [Route("api/app/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly ILogger<AppUserController> _logger;
        private readonly IMediator _mediator; //查询调用处理器
        private readonly DomainNotificationHandler _notificationHandler; //领域通知处理器
        /// <summary>
        /// 
        /// </summary>
        public AppUserController(ILogger<AppUserController> logger, 
            IMediator mediator, 
            INotificationHandler<DomainNotification> notifications)
        {
            _logger = logger;
            _mediator = mediator;
            _notificationHandler = notifications as DomainNotificationHandler;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> LoginAsync([FromBody] LoginUserRequest request)
        {
            try
            {
                // 创建登录命令
                AppUserLoginCommand command = new AppUserLoginCommand(request.Account, request.Password);
                // 发送命令并获取结果
                var result = await _mediator.Send<ResultObject>(command);
                if (result.IsSuccess())
                {
                    _logger.LogInformation("User {Account} logged in successfully", request.Account);

                    return Ok(result);
                }
                else
                {
                    _logger.LogWarning("{Account}登录失败, 原因: {Message}", request.Account, result.message);
                    return Unauthorized(new { message = result.message });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "登录失败，用户账号: {Account}", request.Account);
                return StatusCode(500, new { message = "登录失败" });
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterAsync([FromBody] AddAppUserRequest request)
        {
            var command = new CreateAppUserCommand(request);

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


    }
}
