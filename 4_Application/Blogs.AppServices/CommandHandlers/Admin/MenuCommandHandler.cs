using Blogs.AppServices.Commands.Admin.SysMenu;
using Blogs.Core.Entity.Admin;
using Blogs.Domain.Entity.Admin;
using Blogs.Domain.Enums;
using Blogs.Domain.IRepositorys.Admin;
using Blogs.Domain.Notices;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Blogs.AppServices.CommandHandlers.Admin
{

    /// <summary>
    /// 
    /// </summary>
    public class MenuCommandHandler : CommandHandler,
        IRequestHandler<CreateMenuCommand, bool>,
        IRequestHandler<UpdateMenuCommand, bool>,
        IRequestHandler<DeleteMenuCommand, bool>,
        IRequestHandler<PutMenuButtonCommand, bool>
    {
        private readonly IMenuRepository _menuRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediatorHandler"></param>
        /// <param name="menuRepository"></param>
        public MenuCommandHandler(DomainNotificationHandler mediatorHandler,
            ILogger<MenuCommandHandler> logger,
            IMenuRepository menuRepository)
            : base(mediatorHandler, logger)
        {
            _menuRepository = menuRepository;
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(CreateMenuCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
                return false;
            }
            var entity = command.Adapt<SysMenu>();
            entity.MarkAsCreated(CurrentUser.Instance.UserInfo.UserName);

            //根据传入的按钮ID获取操作按钮
            if (command.Type == (int)MenuButtonTypeEnum.Action)
            {
                if (command.Buttons?.Length == 0)
                {
                    await NotifyError("菜单没有配置操作按钮");
                    return false;
                }
                var menuId = DbContext.Insertable(entity).ExecuteReturnIdentity();

                var buttonIds = command.Buttons;
                var sysButtonList = await DbContext.Queryable<SysButtons>()
                    .Where(it => buttonIds.Contains(it.Id) && it.IsDeleted == 0)
                    .ToListAsync();

                var menuButtonRelations = new List<SysMenuButton>();
                foreach (var item in sysButtonList)
                {
                    var menuButton = new SysMenuButton
                    {
                        MenuId = menuId,
                        ButtonId = item.Id,
                        SortOrder = item.SortOrder
                    };
                    menuButton.MarkAsCreated(CurrentUser.Instance.UserInfo.UserName);
                    menuButtonRelations.Add(menuButton);
                } 
                var result = await DbContext.Insertable(menuButtonRelations).ExecuteCommandAsync();
                return result > 0;
            }
            else
            {
                var result = await DbContext.Insertable(entity).ExecuteCommandAsync();
                return result > 0;
            }
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(UpdateMenuCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
                return await Task.FromResult(false);
            }
            var entity = command.Adapt<SysMenu>();
            entity.MarkAsModified(CurrentUser.Instance.UserInfo.UserName);
            //根据传入的按钮ID获取操作按钮
            if (command.Type == (int)MenuButtonTypeEnum.Action)
            {
                if (command.Buttons?.Length == 0)
                {
                    await NotifyError("菜单没有配置操作按钮");
                    return false;
                } 

                var buttonIds = command.Buttons;
                var sysButtonList = await DbContext.Queryable<SysButtons>()
                    .Where(it => buttonIds.Contains(it.Id) && it.IsDeleted == 0)
                    .ToListAsync();

                var menuButtonRelations = new List<SysMenuButton>();
                foreach (var item in sysButtonList)
                {
                    var menuButton = new SysMenuButton
                    {
                        MenuId = entity.Id,
                        ButtonId = item.Id,
                        SortOrder = item.SortOrder
                    };
                    menuButton.MarkAsCreated(CurrentUser.Instance.UserInfo.UserName);
                    menuButtonRelations.Add(menuButton);
                }

                //删除菜单按钮关系并重新插入。
                var execResult = await DbContext.UseTranAsync(async () =>
                {
                    await DbContext.Deleteable<SysMenuButton>().Where(it => it.MenuId == entity.Id).ExecuteCommandAsync();
                    await DbContext.Updateable(entity).ExecuteCommandAsync();
                    await DbContext.Insertable(menuButtonRelations).ExecuteCommandAsync();
                });
                return execResult.IsSuccess;
            }
            var result = await DbContext.Updateable(entity).ExecuteCommandAsync();
            return result > 0;
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(DeleteMenuCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
                return false;
            }
            var menuInfo = await DbContext.Queryable<SysMenu>().Where(it => it.Id == command.Id).FirstAsync();
            if (menuInfo == null)
            {
                //菜单不存在
                return false;
            }
            var isAuth  = await DbContext.Queryable<SysRoleMenuAuth>().Where(it=>it.MenuId == command.Id).AnyAsync();
            if (isAuth)
            {

            }
            var result = await DbContext.Deleteable(menuInfo).ExecuteCommandAsync();
            return result > 0;
        }

        /// <summary>
        /// 设置菜单按钮
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(PutMenuButtonCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
                return await Task.FromResult(false);
            }
            var menuInfo = await DbContext.Queryable<SysMenu>().Where(it => it.Id == command.Id).FirstAsync();
            if (menuInfo == null)
                return false;

            



            menuInfo.MarkAsModified(CurrentUser.Instance.UserId.ToString());
            var result = await DbContext.Updateable(menuInfo).ExecuteCommandAsync();

            return result > 0;
        }

    }
}
