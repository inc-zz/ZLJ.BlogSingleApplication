using Blogs.AppServices.CommandHandlers.Admin;
using Blogs.AppServices.Commands.Admin.SysUser;
using Blogs.Common.DtoModel.Admin;
using Blogs.Core;
using Blogs.Core.DtoModel.Admin;
using Blogs.Core.Models;
using Blogs.Domain.IRepositorys.Admin;
using Blogs.Domain.IServices;
using Blogs.Domain.Notices;
using Blogs.Infrastructure.OpenIdDict;
using Blogs.Infrastructure.Services.Admin;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace Blogs.AppServices.CommandHandlers
{
    /// <summary>
    /// 用户登录命令处理器
    /// </summary>
    public class UserLoginCommandHandler : CommandHandler,
    IRequestHandler<UserLoginCommand, ResultObject>,
    IRequestHandler<RefreshTokenCommand, ResultObject>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IOpenIddictService _openIddictService;
        private readonly ILogger<UserLoginCommandHandler> _logger;
        private readonly IRedisCacheService _redisCache;

        public UserLoginCommandHandler(
            IUserRepository userRepository,
            IAuthService authService,
            IOpenIddictService openIddictService,
            DomainNotificationHandler notificationHandler,
            ILogger<UserLoginCommandHandler> logger,
            IRedisCacheService redisCache)
             : base(notificationHandler,logger)
        {
            _userRepository = userRepository;
            _authService = authService;
            _openIddictService = openIddictService;
            _redisCache = redisCache;
            _logger = logger;
        }

        /// <summary>
        /// 执行登录
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResultObject> Handle(UserLoginCommand command, CancellationToken cancellationToken)
        {
            var errorMessage = string.Empty;
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
                var errorMessage1 = command.ValidationResult.Errors.Select(e => e.ErrorMessage);
                //发送登录失败通知

                return ResultObject.Error("登录失败，参数错误");
            }

            // 获取用户信息
            var user = await _userRepository.GetByUserNameAsync(command.UserName);
            if (user == null)
            {
                errorMessage = "用户名或密码错误";

                return ResultObject.Error("登录失败，用户名或密码错误");
            }

            // 检查用户是否被锁定
            if (user.IsDeleted)
            {
                errorMessage = "用户已被锁定，请稍后再试";
                return ResultObject.Error("用户已被锁定，请稍后再试");
            }

            // 验证密码
            var pwd = AESCryptHelper.Encrypt(command.Password);
            var isPasswordValid = await _authService.ValidateCredentialsAsync(command.UserName, command.Password);
            if (!isPasswordValid)
            {
                // 增加登录失败次数
                await _userRepository.IncrementLoginFailedCountAsync(user.Id);

                // 检查是否需要锁定账户
                var remainingAttempts = await _authService.GetRemainingLoginAttemptsAsync(user.UserName);
                if (remainingAttempts <= 0)
                {
                    // 锁定账户
                    var unlockTime = DateTime.Now.AddMinutes(30); // 锁定30分钟
                    await _userRepository.LockUserAsync(user.Id, unlockTime);

                    errorMessage = "登录失败次数过多，账户已被锁定30分钟";
                }
                errorMessage = $"用户名或密码错误，剩余尝试次数: {remainingAttempts}";
                return ResultObject.Error(errorMessage);
            }

            // 检查密码是否过期
            if (await _authService.CheckPasswordExpiredAsync(user.Id))
            {
                errorMessage = "密码已过期，请修改密码";
                return ResultObject.Error(errorMessage);
            }

            // 重置登录失败次数
            await _userRepository.ResetLoginFailedCountAsync(user.Id);

            // 更新最后登录信息
            await _userRepository.UpdateLastLoginInfoAsync(user.Id, "127.0.0.1", DateTime.Now);

            // 生成访问令牌 (使用OpenIddict)
            var tokenResult = await _openIddictService.GenerateTokenAsync(user);

            // 返回登录结果
            var loginResult = new LoginResultDto
            {
                UserInfo = user.Adapt<AdminLoginUserDto>(),
                AccessToken = tokenResult.AccessToken,
                RefreshToken = tokenResult.RefreshToken,
                ExpiresIn = tokenResult.Expiration
            };
            return ResultObject<LoginResultDto>.Success(loginResult, "登录成功");
        }

        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResultObject> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("开始刷新令牌");

                // 从令牌中提取用户ID
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(request.AccessToken);

                // 从刷新令牌中获取用户ID
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("无法从刷新令牌获取用户ID: {RefreshToken}", request.RefreshToken);
                    return ResultObject.Error("刷新令牌无效或已过期");
                }

                // 获取用户信息
                var user = await _userRepository.GetFirstAsync(it => it.Id.ToString() == userId);
                if (user == null)
                {
                    _logger.LogWarning("用户不存在: {UserId}", userId);
                    return ResultObject.Error("登录账号异常");
                }

                if (user.IsDeleted)
                {
                    _logger.LogWarning("用户已被锁定: {UserId}", userId);
                    return ResultObject.Error("用户已被锁定，请稍后再试");
                }

                // 使旧的令牌失效
                await _openIddictService.RevokeJwtToken(request.AccessToken, request.RefreshToken);

                // 重新生产令牌，不重置刷新令牌
                var tokenResult = await _openIddictService.GenerateTokenAsync(user, request.RefreshToken);
                if (!tokenResult.Result)
                {
                    _logger.LogError("生成新令牌失败: {Error}", tokenResult.Message);
                    return ResultObject.Error($"生成新令牌失败: {tokenResult.Message}");
                }

                _logger.LogInformation("令牌刷新成功，用户ID: {UserId}", userId);

                return ResultObject<TokenResult>.Success(tokenResult, "");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刷新令牌过程中发生错误");
                return ResultObject.Error($"刷新令牌失败: {ex.Message}");
            }
        }

    }
}
