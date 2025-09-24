using Blogs.AppServices.Commands.Admin.SysUser;
using Blogs.AppServices.Requests.Admin;
using Blogs.AppServices.Responses;
using Blogs.Core.Models;
using Blogs.Infrastructure.Services;
using Blogs.WebApi.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Polly;
using System.IdentityModel.Tokens.Jwt;

namespace Blogs.WebApi.Controllers.Admin
{
    /// <summary>
    /// 账户管理控制器
    /// </summary>
    [ApiController]
    [Route("api/admin/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IMediator _mediator;

        private readonly IOpenIddictService _tokenService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="notification"></param>
        public AccountController(ILogger<AccountController> logger, IOpenIddictService openIddictService,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
            _tokenService = openIddictService;
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
                UserLoginCommand command = new UserLoginCommand(request.Account, request.Password);
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
        /// 刷新令牌
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("refresh")]
        public async Task<ActionResult<UserLoginDto>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                // 创建登录命令
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
                RefreshTokenCommand command = new RefreshTokenCommand(token, request.RefreshToken);
                // 发送命令并获取结果
                var result = await _mediator.Send<ResultObject>(command);
                if (result.IsSuccess())
                {
                    return Ok(result);
                }
                else
                {
                    return Unauthorized(new { message = result.message });
                }
            }
            catch (SecurityTokenException)
            {
                return Unauthorized("Invalid token");
            }
        }
    }
}
