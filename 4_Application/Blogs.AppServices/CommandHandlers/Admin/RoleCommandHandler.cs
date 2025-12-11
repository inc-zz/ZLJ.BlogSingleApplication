using Blogs.AppServices.Commands.Admin.SysDepartment;
using Blogs.AppServices.Commands.Admin.SysRole;
using Blogs.Core.Entity.Admin;
using Blogs.Core.Enums;
using Blogs.Domain.Entity.Admin;
using Blogs.Domain.Notices;
using Microsoft.Extensions.Logging;

namespace Blogs.AppServices.CommandHandlers.Admin
{
    /// <summary>
    /// 角色模块相关命令操作 
    /// </summary>
    public class RoleCommandHandler : CommandHandler,
        IRequestHandler<CreateRoleCommand, bool>,
        IRequestHandler<UpdateRoleCommand, bool>,
        IRequestHandler<DeleteRoleCommand, bool>,
        IRequestHandler<UpdateRoleStatusCommand, bool>,
        IRequestHandler<AuthorizeRoleModulesCommand, bool>,
        IRequestHandler<AuthorizeUserRolesCommand, bool>
    {
        public RoleCommandHandler(
            DomainNotificationHandler notificationHandler,
            ILogger<RoleCommandHandler> logger)
            : base(notificationHandler, logger)
        {
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        public async Task<bool> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command))
                return false;

            return await ExecuteDbOperationAsync(async () =>
            {
                var entity = command.Adapt<SysRole>();
                entity.MarkAsCreated(CurrentUser.Instance.UserInfo.UserName);
                entity.Status = (int)ApproveStatusEnum.Normal;

                var result = await DbContext.Insertable(entity).ExecuteCommandAsync();
                return result > 0;
            }, "创建角色");
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        public async Task<bool> Handle(UpdateRoleCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command))
                return false;

            return await ExecuteDbOperationAsync(async () =>
            {
                var entity = await DbContext.Queryable<SysRole>()
                    .Where(it => it.Id == command.Id && it.IsDeleted == 0)
                    .FirstAsync();

                if (entity == null)
                {
                    await NotifyError("角色不存在");
                    return false;
                }

                // 使用 Mapster 更新实体，避免手动赋值
                entity.Status = 1;
                entity.IsDeleted = 0;
                command.Adapt(entity);
                entity.MarkAsModified(CurrentUser.Instance.UserInfo.UserName);

                var result = await DbContext.Updateable(entity).ExecuteCommandAsync();
                return result > 0;
            }, "修改角色");
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        public async Task<bool> Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command))
                return false;

            return await ExecuteDbOperationAsync(async () =>
            {
                // 1. 检查角色是否存在
                var role = await DbContext.Queryable<SysRole>()
                    .Where(r => r.Id == command.Id && r.IsDeleted == 0)
                    .FirstAsync();

                if (role == null)
                {
                    await NotifyError("角色不存在");
                    return false;
                }

                // 2. 检查是否为系统角色
                if (role.IsSystem == 1 && role.Name == "sysadmin")
                {
                    await NotifyError("系统角色不能删除");
                    return false;
                }

                // 3. 检查角色是否被用户使用
                var userCount = await DbContext.Queryable<SysUserRoleRelation>()
                    .Where(ur => ur.RoleId == command.Id)
                    .CountAsync();

                if (userCount > 0)
                {
                    await NotifyError($"该角色已被 {userCount} 个用户使用，无法删除。请先解除用户关联");
                    return false;
                }

                // 4. 使用事务处理删除操作
                var result = await DbContext.Ado.UseTranAsync(async () =>
                {
                    // 删除角色菜单权限
                    var menuAuthDeleted = await DbContext.Deleteable<SysRoleMenuAuth>()
                        .Where(rm => rm.RoleId == command.Id)
                        .ExecuteCommandAsync();

                    // 软删除角色
                    role.SoftDelete();
                    role.MarkAsModified(CurrentUser.Instance.UserInfo.UserName);

                    var roleDeleted = await DbContext.Updateable(role)
                        .UpdateColumns(r => new { r.IsDeleted, r.ModifiedAt, r.ModifiedBy })
                        .ExecuteCommandAsync();

                    _logger.LogInformation(
                        "角色删除成功: {RoleName}(ID:{RoleId})，删除菜单权限: {MenuAuthCount} 条",
                        role.Name, role.Id, menuAuthDeleted);

                    return roleDeleted > 0;
                });

                return result.IsSuccess && result.Data;
            }, "删除角色");
        }

        /// <summary>
        /// 启用/禁用角色
        /// </summary>
        public async Task<bool> Handle(UpdateRoleStatusCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command))
                return false;

            return await ExecuteDbOperationAsync(async () =>
            {
                // 检查角色是否存在
                if (!await CheckEntityExistsAsync<SysRole>(command.Id, "角色"))
                    return false;

                var result = await DbContext.Updateable<SysRole>()
                    .SetColumns(it => new SysRole { Status = command.Status })
                    .Where(x => x.Id == command.Id && x.IsDeleted == 0)
                    .ExecuteCommandAsync();

                return result > 0;
            }, "更新角色状态");
        }

        /// <summary>
        /// 角色模块授权
        /// </summary>
        public async Task<bool> Handle(AuthorizeRoleModulesCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command))
                return false;


            // 1. 验证角色是否存在
            if (!await CheckEntityExistsAsync<SysRole>(command.RoleId, "角色"))
                return false;

            // 2. 验证菜单是否存在
            var menuIds = command.MenuPermissions.Select(x => x.MenuId).Distinct().ToList();
            var existingMenus = await DbContext.Queryable<SysMenu>()
                .Where(x => menuIds.Contains(x.Id) && x.IsDeleted == 0)
                .Select(x => x.Id)
                .ToListAsync();

            var nonExistingMenus = menuIds.Except(existingMenus).ToList();
            if (nonExistingMenus.Any())
            {
                await NotifyError($"以下菜单不存在: {string.Join(",", nonExistingMenus)}");
                return false;
            }

            // 3. 验证按钮代码格式
            foreach (var menuPermission in command.MenuPermissions)
            {
                if (menuPermission.ButtonCodes?.Any(code => string.IsNullOrWhiteSpace(code)) == true)
                {
                    await NotifyError($"菜单 {menuPermission.MenuId} 的按钮代码不能为空");
                    return false;
                }
            }

            // 4. 使用事务处理权限更新
            var result = await DbContext.Ado.UseTranAsync(async () =>
            {
                // 删除该角色现有的菜单权限
                await DbContext.Deleteable<SysRoleMenuAuth>()
                    .Where(x => x.RoleId == command.RoleId)
                    .ExecuteCommandAsync();

                // 插入新的菜单权限
                var roleMenuAuths = new List<SysRoleMenuAuth>();
                foreach (var menuPermission in command.MenuPermissions)
                {
                    var validButtonCodes = menuPermission.ButtonCodes?
                        .Where(code => !string.IsNullOrWhiteSpace(code))
                        .Distinct()
                        .ToList() ?? new List<string>();

                    var roleMenuAuth = new SysRoleMenuAuth
                    {
                        RoleId = command.RoleId,
                        MenuId = menuPermission.MenuId,
                        ButtonPermissions = string.Join(",", validButtonCodes),
                        CreatedBy = CurrentUser.Instance.UserInfo.UserName,
                        CreatedAt = DateTime.Now
                    };

                    roleMenuAuths.Add(roleMenuAuth);
                }

                if (roleMenuAuths.Any())
                {
                    await DbContext.Insertable(roleMenuAuths).ExecuteCommandAsync();
                }

                return true;
            });

            if (!result.IsSuccess)
            {
                _logger.LogError("角色菜单授权事务失败: {Error}", result.ErrorMessage);
                await NotifyError("角色菜单授权失败，请稍后重试");
                return false;
            }

            _logger.LogInformation("角色菜单授权成功，角色ID: {RoleId}，授权菜单数: {MenuCount}",
                command.RoleId, command.MenuPermissions.Count);

            return true;

        }

        /// <summary>
        /// 用户角色授权
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Handle(AuthorizeUserRolesCommand request, CancellationToken cancellationToken)
        {

            if (!ValidateCommand(request))
                return false;

            return await ExecuteDbOperationAsync(async () =>
            {
                // 1. 验证用户是否存在
                var user = await DbContext.Queryable<SysUser>()
                    .Where(u => u.Id == request.UserId && u.IsDeleted == false)
                    .FirstAsync();

                if (user == null)
                {
                    await NotifyError($"用户不存在，ID: {request.UserId}");
                    return false;
                }

                // 2. 验证角色是否存在
                var role = await DbContext.Queryable<SysRole>()
                    .Where(r => r.Id == request.RoleId && r.IsDeleted == 0 && r.Status == (int)ApproveStatusEnum.Normal)
                    .FirstAsync();

                if (role == null)
                {
                    await NotifyError($"角色不存在或已禁用，ID: {request.RoleId}");
                    return false;
                }

                // 3. 使用事务处理角色授权
                var result = await DbContext.Ado.UseTranAsync(async () =>
                {
                    // 删除用户现有的所有角色关联
                    var deletedCount = await DbContext.Deleteable<SysUserRoleRelation>()
                            .Where(ur => ur.UserId == request.UserId)
                            .ExecuteCommandAsync();

                    _logger.LogInformation("删除用户 {UserId} 的 {DeletedCount} 条原有角色关联",
                    request.UserId, deletedCount);

                    // 插入新的角色关联
                    var userRoleRelation = new SysUserRoleRelation
                    {
                        UserId = request.UserId,
                        RoleId = request.RoleId
                    };
                    var insertedCount = await DbContext.Insertable(userRoleRelation).ExecuteCommandAsync();

                    _logger.LogInformation("为用户 {UserId} 授权角色 {RoleId}({RoleName})",
                    request.UserId, role.Id, role.Name);

                    return insertedCount > 0;
                });

                if (!result.IsSuccess)
                {
                    _logger.LogError("用户角色授权事务失败: {Error}", result.ErrorMessage);
                    await NotifyError("用户角色授权失败，请稍后重试");
                    return false;
                }
                _logger.LogInformation("用户角色授权成功，用户ID: {UserId}，角色ID: {RoleId}",
                                request.UserId, request.RoleId);
                return true;
            }, "用户角色授权");
        }
    }
}