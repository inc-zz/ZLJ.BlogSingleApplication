using Blogs.AppServices.Commands.Blogs.User;
using Blogs.Common.DtoModel.App;
using Blogs.Core;
using Blogs.Core.Models;
using Blogs.Domain.IRepositorys.Blogs;
using Blogs.Domain.IServices;
using Blogs.Infrastructure.Services.App;

namespace Blogs.AppServices.CommandHandlers.App
{
    /// <summary>
    /// App用户登录
    /// </summary>
    public class AppUserLoginCommandHandler : AppCommandHandler,
         IRequestHandler<AppUserLoginCommand, ResultObject>
    {
        private readonly IAppUserRepository _userRepository;
        private readonly IAppAuthService _authService;
        private readonly IMediatorHandler _eventBus;
        private readonly IAppOpenIddictService _appOpenIddictService;


        public AppUserLoginCommandHandler(IMediatorHandler mediator,
            IAppAuthService appAuthService,
            IAppUserRepository appUserRepository,
            IAppOpenIddictService appOpenIddictService)
            : base(mediator)
        {
            _eventBus = mediator;
            _authService = appAuthService;
            _userRepository = appUserRepository;
            _appOpenIddictService = appOpenIddictService;
        }

        /// <summary>
        /// 执行登录
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResultObject> Handle(AppUserLoginCommand command, CancellationToken cancellationToken)
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
            var user = await _userRepository.GetFirstAsync(it => it.Account == command.Account);
            if (user == null)
            {
                errorMessage = "用户名或密码错误";

                return ResultObject.Error("登录失败，用户名或密码错误");
            }

            // 检查用户是否被锁定
            if (user.IsDeleted == 1)
            {
                errorMessage = "用户已被锁定，请稍后再试";
                return ResultObject.Error("用户已被锁定，请稍后再试");
            }

            // 验证密码
            var pwd = AESCryptHelper.Encrypt(command.Password);
            var isPasswordValid = await _authService.ValidateCredentialsAsync(command.Account, command.Password);
            if (!isPasswordValid)
            {
                // 增加登录失败次数
                //await _userRepository.IncrementLoginFailedCountAsync(user.Id);

                // 检查是否需要锁定账户
                var remainingAttempts = await _authService.GetRemainingLoginAttemptsAsync(user.Account);
                if (remainingAttempts <= 0)
                {
                    // 锁定账户
                    var unlockTime = DateTime.Now.AddMinutes(30); // 锁定30分钟
                    //await _userRepository.LockUserAsync(user.Id, unlockTime);

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
            //await _userRepository.ResetLoginFailedCountAsync(user.Id);

            // 更新最后登录信息
            await _userRepository.UpdateLastLoginInfoAsync(user.Id, "127.0.0.1", DateTime.Now);

            // 生成访问令牌 (使用OpenIddict)
            var tokenResult = await _appOpenIddictService.GenerateTokenAsync(user);

            // 返回登录结果
            var loginResult = new AppLoginResultDto
            {
                UserInfo = user.Adapt<AppLoginUserDto>(),
                AccessToken = tokenResult.AccessToken,
                RefreshToken = tokenResult.RefreshToken,
                ExpiresIn = tokenResult.Expiration
            };
            return ResultObject<AppLoginResultDto>.Success(loginResult, "登录成功");
        }




    }
}
