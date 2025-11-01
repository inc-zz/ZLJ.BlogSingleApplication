using Blogs.AppServices.Commands.Admin.SysButtons;
using Blogs.Domain.Entity.Admin;
using Blogs.Domain.Notices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.CommandHandlers.Admin
{
    /// <summary>
    /// 按钮命令执行器
    /// </summary>
    public class SysButtonCommandHandler : CommandHandler,
        IRequestHandler<SysCreateButtonCommand, bool>,
        IRequestHandler<SysUpdateButtonCommand, bool>,
        IRequestHandler<SysDeleteButtonCommand, bool>,
        IRequestHandler<SysUpdateButtonStatusCommand, bool>
    {
        private readonly ILogger<SysButtonCommandHandler> _logger;

        public SysButtonCommandHandler(
            DomainNotificationHandler notificationHandler,
            ILogger<SysButtonCommandHandler> logger)
            : base(notificationHandler, logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 创建按钮
        /// </summary>
        public async Task<bool> Handle(SysCreateButtonCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command))
                return false;

            // 检查按钮代码是否已存在
            var exists = await DbContext.Queryable<SysButtons>()
                .Where(b => b.Code == command.Code && b.IsDeleted == 0)
                .AnyAsync();

            if (exists)
            {
                await NotifyError($"按钮代码 '{command.Code}' 已存在");
                return false;
            }

            var entity = command.Adapt<SysButtons>();
            entity.MarkAsCreated(CurrentUser.Instance.UserInfo.UserName);

            var result = await DbContext.Insertable(entity).ExecuteCommandAsync();

            if (result > 0)
            {
                _logger.LogInformation("创建按钮成功: {ButtonName}({ButtonCode})", entity.Name, entity.Code);
            }
            return result > 0;

        }

        /// <summary>
        /// 更新按钮
        /// </summary>
        public async Task<bool> Handle(SysUpdateButtonCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command))
                return false;

            var entity = await DbContext.Queryable<SysButtons>()
                      .Where(b => b.Id == command.Id && b.IsDeleted == 0)
                      .FirstAsync();

            if (entity == null)
            {
                await NotifyError("按钮不存在");
                return false;
            }

            // 检查按钮代码是否被其他按钮使用
            var codeExists = await DbContext.Queryable<SysButtons>()
                .Where(b => b.Code == command.Code && b.Id != command.Id && b.IsDeleted == 0)
                .AnyAsync();

            if (codeExists)
            {
                await NotifyError($"按钮代码 '{command.Code}' 已被其他按钮使用");
                return false;
            }

            command.Adapt(entity);
            entity.MarkAsModified(CurrentUser.Instance.UserInfo.UserName);
            var result = await DbContext.Updateable(entity).ExecuteCommandAsync();

            if (result > 0)
            {
                _logger.LogInformation("更新按钮成功: {ButtonName}({ButtonCode})", entity.Name, entity.Code);
            }

            return result > 0;

        }

        /// <summary>
        /// 删除按钮
        /// </summary>
        public async Task<bool> Handle(SysDeleteButtonCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command))
                return false;

            return await ExecuteDbOperationAsync(async () =>
            {
                var entity = await DbContext.Queryable<SysButtons>()
                    .Where(b => b.Id == command.Id && b.IsDeleted == 0)
                    .FirstAsync();

                if (entity == null)
                {
                    await NotifyError("按钮不存在");
                    return false;
                }

                // 检查按钮是否被菜单使用
                var usedByMenus = await DbContext.Queryable<SysMenuButton>()
                    .Where(mb => mb.ButtonId == command.Id)
                    .CountAsync();

                if (usedByMenus > 0)
                {
                    await NotifyError($"按钮 '{entity.Name}' 已被 {usedByMenus} 个菜单使用，无法删除");
                    return false;
                }

                //// 检查按钮是否被角色权限使用
                //var usedByRoles = await DbContext.Queryable<SysRoleButtonAuth>()
                //    .Where(rb => rb.ButtonId == command.Id)
                //    .CountAsync();

                //if (usedByRoles > 0)
                //{
                //    await NotifyError($"按钮 '{entity.Name}' 已被 {usedByRoles} 个角色权限使用，无法删除");
                //    return false;
                //}

                // 软删除
                entity.SoftDelete();
                entity.MarkAsModified(CurrentUser.Instance.UserInfo.UserName);

                var result = await DbContext.Updateable(entity)
                    .UpdateColumns(b => new { b.IsDeleted, b.ModifiedAt, b.ModifiedBy })
                    .ExecuteCommandAsync();

                if (result > 0)
                {
                    _logger.LogInformation("删除按钮成功: {ButtonName}({ButtonCode})", entity.Name, entity.Code);
                }

                return result > 0;
            }, "删除按钮");
        }

        /// <summary>
        /// 更新按钮状态
        /// </summary>
        public async Task<bool> Handle(SysUpdateButtonStatusCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command))
                return false;

            return await ExecuteDbOperationAsync(async () =>
            {
                if (!await CheckEntityExistsAsync<SysButtons>(command.Id, "按钮"))
                    return false;

                var result = await DbContext.Updateable<SysButtons>()
                    .SetColumns(b => new SysButtons { Status = command.Status })
                    .Where(b => b.Id == command.Id)
                    .ExecuteCommandAsync();

                if (result > 0)
                {
                    var statusText = command.Status == 1 ? "启用" : "禁用";
                    _logger.LogInformation("更新按钮状态成功: 按钮ID {ButtonId} -> {Status}", command.Id, statusText);
                }

                return result > 0;
            }, "更新按钮状态");
        }
    }
}
