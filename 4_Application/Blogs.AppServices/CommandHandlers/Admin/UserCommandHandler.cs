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
using Blogs.Domain.Notices;
using Blogs.Domain.ValueObject;
using Blogs.Infrastructure.Context;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NCD.Common;
using Newtonsoft.Json;

namespace Blogs.AppServices.CommandHandlers.Admin
{

    /// <summary>
    /// 用户模块相关命令处理
    /// </summary>
    public class AppUserCommandHandler : CommandHandler,
        //IRequestHandler<UserLoginCommand, bool>,
        IRequestHandler<LoginOutCommand, bool>,
        IRequestHandler<CreateUserCommand, bool>,
        IRequestHandler<UpdateUserCommand, bool>,
        IRequestHandler<DeleteUserCommand, bool>,
        IRequestHandler<BeathDeleteUserCommand, bool>,
        IRequestHandler<ChangeUserStatusCommand, bool>,
        IRequestHandler<UserRoleAuthCommand, bool>,
        IRequestHandler<ResetPasswordCommand, bool>
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
        public AppUserCommandHandler( 
            IHttpContextAccessor httpContext,
            IMediatorHandler mediatorHandler,
            DomainNotificationHandler notificationHandler,
            ILogger<AppUserCommandHandler> logger,
            IUserRepository userRepository)
            : base(notificationHandler,logger)
        {
            _httpContext = httpContext;
            _userRepository = userRepository;
            _eventBus = mediatorHandler;
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
            user.Id = new IdWorkerUtils().NextId();
            //创建人信息
            user.MarkAsCreated(CurrentUser.Instance.UserInfo.UserName);
            user.Status = (int)ApproveStatusEnum.Normal;

            if (!string.IsNullOrWhiteSpace(command.DepartmentJson))
            {
                var userDepartment = JsonConvert.DeserializeObject<UserDepartment>(command.DepartmentJson);
                user.DepartmentId = userDepartment.Id;
            }

            //用户角色关系
            if (!string.IsNullOrWhiteSpace(command.UserRoleJson))
            {
                List<UserRoleModel> userRole = JsonConvert.DeserializeObject<List<UserRoleModel>>(command.UserRoleJson);
                user.UserRoles = userRole.Adapt<List<SysUserRoleRelation>>();

            }
            List<SysUserRoleRelation> userRoleRelations = user.UserRoles;
            foreach (var item in userRoleRelations)
            {
                item.UserId = user.Id;
            }
            var result = await DbContext.UseTranAsync(async () =>
            {
                await DbContext.Insertable(user).ExecuteCommandAsync();
                await DbContext.Insertable(user.UserRoles).ExecuteCommandAsync();
            });

            return await Task.FromResult(result.IsSuccess);
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

            entity.RealName = command.RealName;
            entity.Sex = command.Sex;
            entity.HeadPic = command.HeadPic;
            entity.PhoneNumber = command.PhoneNumber;
            entity.Email = command.Email;
            entity.Description = command.Description;

            //修改人信息
            entity.MarkAsModified(CurrentUser.Instance.UserInfo.UserName);
            entity.Status = (int)ApproveStatusEnum.Normal;

            if (!string.IsNullOrWhiteSpace(command.DepartmentJson))
            {
                var userDepartment = JsonConvert.DeserializeObject<UserDepartment>(command.DepartmentJson);
                entity.DepartmentId = userDepartment.Id;
            }

            //用户角色关系
            if (!string.IsNullOrWhiteSpace(command.UserRoleJson))
            {
                List<UserRoleModel> userRole = JsonConvert.DeserializeObject<List<UserRoleModel>>(command.UserRoleJson);
                entity.UserRoles = userRole.Adapt<List<SysUserRoleRelation>>();

            }
            List<SysUserRoleRelation> userRoleRelations = entity.UserRoles;
            foreach (var item in userRoleRelations)
            {
                item.UserId = entity.Id;
            }
            var result = await DbContext.UseTranAsync(async () =>
            {
                await DbContext.Deleteable<SysUserRoleRelation>().Where(it => it.UserId == entity.Id).ExecuteCommandAsync();
                await DbContext.Updateable(entity).ExecuteCommandAsync();
                await DbContext.Insertable(entity.UserRoles).ExecuteCommandAsync();
            });

            return result.IsSuccess;
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
            var currentUserName = CurrentUser.Instance.UserInfo.UserName;
            var user = await DbContext.Queryable<SysUser>().FirstAsync(x => x.Id == command.Id);
            if (user == null)
            {
                await _eventBus.RaiseEvent(new DomainNotification("deleteUser", "用户不存在！"));
                return await Task.FromResult(false);
            }
            if (user.UserName == "admin" || user.UserName == currentUserName)
            {
                await _eventBus.RaiseEvent(new DomainNotification("deleteUser", "无法删除管理员"));
                return await Task.FromResult(false);
            }
            user.SoftDelete();
            user.Status = (int)ApproveStatusEnum.Disable; //删除的账号禁用登录
            user.MarkAsModified(currentUserName);

            var result = await DbContext.Updateable(user).ExecuteCommandAsync();
            if (result > 0)
                return await Task.FromResult(true);
            return await Task.FromResult(false);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Handle(BeathDeleteUserCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
                return await Task.FromResult(false);
            }
            var userList = await DbContext.Queryable<SysUser>()
                .Where(x => command.Ids.Contains(x.Id)).ToListAsync();

            if (userList.Count == 0)
            {
                await _eventBus.RaiseEvent(new DomainNotification("deleteUser", "用户不存在！"));
                return await Task.FromResult(false);
            }
            var currentUserName = CurrentUser.Instance.UserInfo.UserName;
            var existsAdmin = userList.Where(x => x.UserName == "admin" || x.UserName == currentUserName).Any();
            if (existsAdmin)
            {
                await _eventBus.RaiseEvent(new DomainNotification("deleteUser", "无法删除管理员"));
                return await Task.FromResult(false);
            }
            foreach (var user in userList)
            {
                user.SoftDelete();
                user.Status = (int)ApproveStatusEnum.Disable; //删除的账号禁用登录
                user.MarkAsModified(currentUserName);
            }
            var result = await DbContext.Updateable(userList).ExecuteCommandAsync();
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
            user.ResetStatus();
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

            var userRole = JsonConvert.DeserializeObject<List<UserRoleModel>>(command.UserRoleJson);
            user.UserRoles = userRole.Adapt<List<SysUserRoleRelation>>();
            foreach (var item in user.UserRoles)
            {
                item.UserId = user.Id;
            }
            var result = await DbContext.UseTranAsync(async () =>
            {
                await DbContext.Deleteable<SysUserRoleRelation>().Where(it => it.UserId == user.Id).ExecuteCommandAsync();
                await DbContext.Insertable(user.UserRoles).ExecuteCommandAsync();
            });
            return result.IsSuccess;
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
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
            user.Password = command.Password;
            var userName = CurrentUser.Instance.UserInfo.UserName;
            user.MarkAsModified(userName);
            var result = await DbContext.Updateable(user).ExecuteCommandAsync();
            return result > 0;
        }
    }
}
