using Blogs.AppServices.Commands.Admin.AppUser;
using Blogs.AppServices.Commands.Admin.SysUser;
using Blogs.AppServices.Commands.Blogs.User;
using Blogs.Core.Entity.Blogs;
using Blogs.Core.Enums;
using Blogs.Domain.IRepositorys.Blogs;
using Microsoft.AspNetCore.Http;
using NCD.Common;
using Newtonsoft.Json;

namespace Blogs.AppServices.CommandHandlers.App
{

    /// <summary>
    /// 用户模块相关命令处理
    /// </summary>
    public class AppUserCommandHandler : AppCommandHandler,
        IRequestHandler<CreateAppUserCommand, bool>,
        IRequestHandler<UpdateAppUserCommand, bool>,
        IRequestHandler<ChangeAppUserStatusCommand, bool>,
        IRequestHandler<ResetAppUserPasswordCommand, bool> 
    {
        private readonly IMediatorHandler _eventBus;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IAppUserRepository _userRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventBus"></param>
        /// <param name="httpContext"></param>
        /// <param name="userRepository"></param>
        public AppUserCommandHandler(IMediatorHandler eventBus,
            IHttpContextAccessor httpContext,
            IAppUserRepository userRepository)
            : base(eventBus)
        {
            _eventBus = eventBus;
            _httpContext = httpContext;
            _userRepository = userRepository;
        }

        /// <summary>
        /// 注册账号
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(CreateAppUserCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
                return await Task.FromResult(false);
            }
            try
            {
                var user = command.Adapt<BlogsUser>();
                user.Id = new IdWorkerUtils().NextId();

                //创建人信息
                user.CreatedAt = DateTime.Now;
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine(JsonConvert.SerializeObject(user));
                Console.WriteLine("------------------------------------------------------------");

                var isTrue = await _userRepository.InsertAsync(user);
                if (isTrue)
                    return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            return await Task.FromResult(false);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(UpdateAppUserCommand command, CancellationToken cancelToken)
        {

            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
                return await Task.FromResult(false);
            }

            var user = await _userRepository.GetByIdAsync(command.Id);
            if (user == null)
            {
                return await Task.FromResult(false);
            }
            //头像设置
            user.SetAvatar(command.Avatar);
            user.SetEmail(command.Email);
            user.Description = command.Description;

            var isTrue = await _userRepository.UpdateAsync(user);
            return isTrue;
        }

        /// <summary>
        /// 启用/禁用用户
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Handle(ChangeAppUserStatusCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
                return await Task.FromResult(false);
            }

            var user = await _userRepository.GetByIdAsync(command.Id);
            if (user == null)
            {
                return await Task.FromResult(false);
            }
            if (command.Status == 1)
            {
                user.Enable();
            }
            else
            {
                user.Disable();
            }

            var isTrue = await _userRepository.UpdateAsync(user);
            return isTrue;

        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Handle(ResetAppUserPasswordCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
                return false;
            }
            var user = await _userRepository.GetByIdAsync(command.Id);
            if (user == null)
            {
                return false;
            }
            user.ResetPwd(command.NewPassword);

            var isTrue = await _userRepository.UpdateAsync(user);
            return isTrue;
        }

    }
}
