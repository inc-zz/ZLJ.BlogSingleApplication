using Blogs.AppServices.Commands.Blogs.AppUser;
using Blogs.AppServices.Commands.Blogs.User;
using Blogs.AppServices.Queries.App;
using Blogs.AppServices.Requests.Admin;
using Blogs.AppServices.Requests.App;
using Blogs.AppServices.Responses;
using Blogs.Core.Models;
using Blogs.Domain.EventNotices;
using Blogs.Domain.Notices;
using Blogs.Infrastructure.Context;
using Blogs.WebApi.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens; 

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
            var command = new CreateBlogAppUserCommand(request);
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
        /// 更新个人信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<ActionResult> SetUserInfoAsync([FromBody] EditAppUserRequest request)
        {
            var updateAppUserCommand = new UpdateAppUserCommand(request.Id,request.Email,request.Avatar,request.Remark,request.PhoneNumber);
            var result = await _mediator.Send(updateAppUserCommand);
            return Ok(ResultObject.Success("处理成功"));
        }

        /// <summary>
        /// 获取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("current")]
        public async Task<ActionResult> GetCurrentUserAsync()
        {
            var query = new GetAppUserInfoQuery
            {
                Id = CurrentAppUser.Instance.UserId
            };
            var result = await _mediator.Send(query);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// 个人主页信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("user/homepage")]
        public async Task<ActionResult> GetUserHomePageAsync([FromQuery] GetUserHomePageQuery request)
        {
            var result = await _mediator.Send(request);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("refreshToken")]
        public async Task<ActionResult<UserLoginDto>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                // 创建登录命令
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
                AppRefreshTokenCommand command = new AppRefreshTokenCommand(token, request.RefreshToken);
                // 发送命令并获取结果
                var result = await _mediator.Send<ResultObject>(command);
                if (result.IsSuccess())
                {
                    return Ok(result);
                }
                else
                {
                    result.code = 401;
                    result.message = "令牌无效或已过期";
                    return Unauthorized(result);
                }
            }
            catch (SecurityTokenException)
            {
                return Unauthorized("Invalid token");
            }
        }
   
    }
}
