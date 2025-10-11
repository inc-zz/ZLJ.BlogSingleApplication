using Blogs.AppServices.Commands.Admin.SysMenu;
using Blogs.Core.Entity.Admin;
using Blogs.Domain.Entity.Admin;
using Blogs.Domain.IRepositorys.Admin;
using Mapster;
using MediatR;

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
        private readonly IMediatorHandler _mediatorHandler;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediatorHandler"></param>
        /// <param name="menuRepository"></param>
        public MenuCommandHandler(IMediatorHandler mediatorHandler, IMenuRepository menuRepository) : base(mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
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
                return await Task.FromResult(false);
            }
            var entity = command.Adapt<SysMenu>();
            entity.MarkAsCreated(CurrentUser.Instance.UserInfo.UserName);

            if (command.Buttons?.Count > 0)
                entity.Buttons = string.Join(",", command.Buttons.Select(x => x.ButtonId).ToList());
            else
                entity.Buttons = string.Empty;
            var result = await DbContext.Insertable(entity).ExecuteCommandAsync();

            return result > 0;
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

            if (command.Buttons?.Count > 0)
                entity.Buttons = string.Join(",", command.Buttons.Select(x => x.ButtonId).ToList());
            else
                entity.Buttons = string.Empty;

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
                return await Task.FromResult(false);
            }
            var isDeleted = await _menuRepository.DeleteAsync(x => x.Id == command.Id);
            if (isDeleted)
            {
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
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
            
            var menuIdList = command.Buttons.Select(x => x.ButtonId).ToList();
            var menuButtons = string.Join(",", menuIdList);

            menuInfo.Buttons = menuButtons;
            menuInfo.MarkAsModified(CurrentUser.Instance.UserId.ToString());
            var result = await DbContext.Updateable(menuInfo).ExecuteCommandAsync();

            return result > 0;
        }

    }
}
