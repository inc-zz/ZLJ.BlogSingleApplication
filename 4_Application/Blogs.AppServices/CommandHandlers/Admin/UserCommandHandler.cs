using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blogs.AppServices.Commands.Admin.SysUser;
using Blogs.Core.Entity.Admin;
using Blogs.Core.Enums;
using Blogs.Domain.Entity.Admin;
using Blogs.Domain.EventNotices;
using Blogs.Domain.IRepositorys.Admin;
using Blogs.Infrastructure.Context;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Blogs.AppServices.CommandHandlers.Admin
{

    /// <summary>
    /// 用户模块相关命令处理
    /// </summary>
    public class UserCommandHandler : CommandHandler,
        //IRequestHandler<UserLoginCommand, bool>,
        IRequestHandler<LoginOutCommand, bool>,
        IRequestHandler<CreateUserCommand, bool>,
        IRequestHandler<UpdateUserCommand, bool>,
        IRequestHandler<DeleteUserCommand, bool>,
        IRequestHandler<ChangeUserStatusCommand, bool>,
        IRequestHandler<UserRoleAuthCommand, bool>
    {
        private readonly IMediatorHandler _eventBus;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventBus"></param>
        /// <param name="httpContext"></param>
        /// <param name="userRepository"></param>
        public UserCommandHandler(IMediatorHandler eventBus, 
            IHttpContextAccessor httpContext, 
            IUserRepository userRepository)
            : base(eventBus)
        {
            _eventBus = eventBus;
            _httpContext = httpContext;
            _userRepository = userRepository;
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> Handle(LoginOutCommand command, CancellationToken cancellationToken)
        {
            var tokenStr = string.Empty;
            if (_httpContext.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                var tokenHeader = _httpContext.HttpContext.Request.Headers["Authorization"];
                tokenStr = tokenHeader.ToString().Substring("Bearer ".Length).Trim();
            }

            if (!string.IsNullOrWhiteSpace(tokenStr))
            {
                //var token = JwtService.SerializeJwt(tokenStr);
                //var cacheKey = $"{SqrayConfig.Instance.baseConfig.LoginKey}{token.UserName}";

                ////清空Redis
                //var tokenModel = _cacheService.GetCache<TokenModel>(cacheKey);
                //_cacheService.RemoveCache(cacheKey);

            }
            return Task.FromResult(true);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
                return await Task.FromResult(false);
            }
            var user = command.Adapt<SysUser>();

            //创建人信息
            //user.MarkAsCreated(CurrentUser.Instance.Account);
            user.Status = (int)ApproveStatusEnum.Normal;

            var isTrue = await _userRepository.InsertAsync(user);
            if (isTrue)
                return await Task.FromResult(true);
            return await Task.FromResult(false);
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>    
        /// <returns></returns>
        public async Task<bool> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
                return await Task.FromResult(false);
            }

            var entity = await _userRepository.GetFirstAsync(x => x.Id == command.Id);
            if (entity == null)
                return await Task.FromResult(false);

            //entity = command.Adapt<SysUser>();

            entity.RealName = command.RealName;
            entity.Sex = command.Sex;
            entity.HeadPic = command.HeadPic;
            entity.PhoneNumber = command.PhoneNumber;
            entity.Email = command.Email;
            entity.Status = command.Status;
            entity.Description = command.Description;

            entity.MarkAsModified(CurrentUser.Instance.UserInfo.UserName);

            var isUpdate = await _userRepository.UpdateAsync(entity);
            if (isUpdate)
                return await Task.FromResult(true);
            return await Task.FromResult(false);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns></returns>
        public async Task<bool> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
                return await Task.FromResult(false);
            }

            var user = await DbContext.Queryable<SysUser>().FirstAsync(x => x.Id == command.Id);
            if (user == null)
            {
                await _eventBus.RaiseEvent(new DomainNotification("deleteUser", "用户不存在！"));
                return await Task.FromResult(false);
            }
            var existsAdmin = await DbContext.Queryable<SysRole>().AnyAsync(x => x.Id == command.RoleId && x.IsSystem == 1);
            if (existsAdmin)
            {
                await _eventBus.RaiseEvent(new DomainNotification("deleteUser", "无法删除管理员"));
                return await Task.FromResult(false);
            }
             
            user.Status = (int)ApproveStatusEnum.Delete;
            user.MarkAsModified(CurrentUser.Instance.UserInfo.UserName);
           
            var result = await DbContext.Updateable(user).ExecuteCommandAsync();
            if (result > 0)
                return await Task.FromResult(true);
            return await Task.FromResult(false);
        }
        /// <summary>
        /// 禁用启用
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(ChangeUserStatusCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
                return await Task.FromResult(false);
            }
            var user = await DbContext.Queryable<SysUser>().FirstAsync(x => x.Id == command.Id);
            if (user == null)
            {
                await _eventBus.RaiseEvent(new DomainNotification("changeUserStatus", "用户不存在！"));
                return await Task.FromResult(false);
            }
            user.Status = command.Status;
            user.MarkAsModified(CurrentUser.Instance.UserInfo.UserName);
            var result = await DbContext.Updateable(user).ExecuteCommandAsync();
            if (result > 0)
                return await Task.FromResult(true);
            return await Task.FromResult(false);

        }

        /// <summary>
        /// 用户授权角色
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(UserRoleAuthCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
                return await Task.FromResult(false);
            }
            var user = await DbContext.Queryable<SysUser>().FirstAsync(x => x.Id == command.Id);
            if (user == null)
            {
                await _eventBus.RaiseEvent(new DomainNotification("userTenantAuth", "用户不存在！"));
                return await Task.FromResult(false);
            }
            var validate = DbContext.Queryable<SysPermissions>().Any(x => x.RoleId == command.RoleId && x.MenuId > 0);
            if (!validate)
            {
                await _eventBus.RaiseEvent(new DomainNotification("userTenantAuth", "该角色没有对应的菜单权限！"));
                return await Task.FromResult(false);
            }

            //user.RoleIds = command.RoleId.ToString();
            
            //var result = await DbContext.Updateable(user).ExecuteCommandAsync();
            //if (result > 0)
            //{
            //    //清除Redis缓存
            //    var cacheKey = $"{SqrayConfig.Instance.baseConfig.LoginKey}{user.Account}";
            //    _cacheService.RemoveCache(cacheKey);
            //    return await Task.FromResult(true);
            //}
            return await Task.FromResult(false);

        }
    }
}
