using Blogs.AppServices.Commands.Admin.AuthManager;
using Blogs.AppServices.Queries.ResponseDto.Admin;
using Blogs.AppServices.Responses;
using Blogs.Domain.Entity.Admin;
using Blogs.Domain.Notices;
using Blogs.Domain.ValueObject;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.CommandHandlers.Admin
{
    public class AuthPermissionsCommandHandler : CommandHandler,
        IRequestHandler<AuthRoleMenuCommand, bool>
    {
        public AuthPermissionsCommandHandler(DomainNotificationHandler mediatorHandler,
             ILogger<AuthPermissionsCommandHandler> logger) : base(mediatorHandler, logger)
        {

        }

        /// <summary>
        /// 角色菜单按钮授权
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(AuthRoleMenuCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
                return false;
            }
            // 1. 验证角色是否存在
            var roleExists = await DbContext.Queryable<SysRole>()
                .Where(x => x.Id == command.RoleId && x.IsDeleted == 0)
                .AnyAsync(cancellationToken);

            if (!roleExists)
            {
                await NotifyError("角色不存在");
                return false;
            }

            // 2. 验证菜单是否存在
            var menuIds = command.RoleMenus.Select(r => r.MenuId).ToList();
            var menuList = await DbContext.Queryable<SysMenu>()
                .Where(x => menuIds.Contains(x.Id) && x.IsDeleted == 0)
                .ToListAsync(cancellationToken);

            if (menuList.Count != menuIds.Count)
            {
                var missingMenus = menuIds.Except(menuList.Select(it => it.Id)).ToList();
                await NotifyError($"以下菜单不存在或已被删除: {string.Join(",", missingMenus)}");
                return false;
            }

            //3、找出授权按钮并验证
            var allButtonIds = command.RoleMenus.SelectMany(rm => rm.ButtonIds).Distinct().ToList();
            var buttonList = await DbContext.Queryable<SysButtons, SysMenuButton>((a, b) => a.Id == b.ButtonId)
                .Where((a, b) => allButtonIds.Contains(a.Id) && a.IsDeleted == 0 && menuIds.Contains(b.MenuId))
                .Select((a, b) => new SysRoleMenuButtonDto
                {
                    RoleId = command.RoleId,
                    MenuId = b.MenuId,
                    ButtonId = a.Id,
                    ButtonCode = a.Code,
                    ButtonName = a.Name
                })
                .ToListAsync();

            //5、存储角色-菜单-按钮关系表，实现授权
            var sysRolemenuAuth = new List<SysRoleMenuAuth>();
            foreach (var item in menuList)
            {
                var authButtonList = buttonList.Where(it => it.MenuId == item.Id)
                    .Select(x => new AuthButtonObject
                    {
                        Id = x.ButtonId,
                        Code = x.ButtonCode,
                        Name = x.ButtonName
                    }).ToList();

                var authModel = new SysRoleMenuAuth
                {
                    RoleId = command.RoleId,
                    MenuId = item.Id,
                    ButtonPermissions = JsonConvert.SerializeObject(authButtonList)
                };
                authModel.MarkAsCreated(CurrentUser.Instance.UserInfo.UserName);
                sysRolemenuAuth.Add(authModel);
            }

            // 4. 执行授权操作（使用事务确保数据一致性）
            var result = await DbContext.UseTranAsync(async () =>
            {
                //删除当前角色授权配置
                await DbContext.Deleteable<SysRoleMenuAuth>().Where(it => it.RoleId == command.RoleId).ExecuteCommandAsync();
                //重新配置角色权限
                await DbContext.Insertable(sysRolemenuAuth).ExecuteCommandAsync();
            });
            return result.IsSuccess;
        }

    }
}
