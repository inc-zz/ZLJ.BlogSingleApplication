using Blogs.AppServices.Commands.Admin.AppUser;
using Blogs.AppServices.Commands.Blogs.User;
using Blogs.AppServices.Queries.App;
using Blogs.AppServices.Requests.Admin;
using Blogs.Core.Models;
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
        private readonly ILogger<AccountController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        public AppUserController(IMediator mediator, ILogger<AccountController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<ActionResult> GetUserListAsync([FromQuery] GetAppUserListRequest param)
        {
            // 创建查询对象
            var query = new GetAppUserListQuery
            {
                PageIndex = param.PageIndex,
                PageSize = param.PageSize,
                Status = param.Status,
                Where = param.Where
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
            return Ok(ResultObject.Success(result ? "处理成功" : "处理失败"));
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<ActionResult> UpdateUserAsync([FromBody] CreateUserRequest param)
        {
            var command = new CreateAppUserCommand(param);
            var result = await _mediator.Send(command);
            return Ok(ResultObject.Success(result ? "处理成功" : "处理失败"));
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<ActionResult> UpdateUserAsync([FromBody] UpdateAppUserRequest param)
        {
            var command = new UpdateAppUserCommand(param.Id, param.Email, param.Avatar, param.PhoneNumber, param.Remark);
            var result = await _mediator.Send(command);
            return Ok(ResultObject.Success(result ? "处理成功" : "处理失败"));
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("resetpwd")]
        public async Task<ActionResult> ResetUserPasswordAsync([FromBody] ResetAppUserPasswordRequest request)
        {
            var command = new ResetAppUserPasswordCommand(request.Id, request.Password);
            var result = await _mediator.Send(command);
            return Ok(ResultObject.Success(result ? "处理成功" : "处理失败"));
        }

    }
}
