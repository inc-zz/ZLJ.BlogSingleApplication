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
            //发送创建菜单命令

            return await Task.FromResult(false);
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
            //执行修改菜单命令，调用


            return await Task.FromResult(false);
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

            var menuIdList = command.Buttons.Select(x => x.ButtonId).ToList();
            var menuButtons = string.Join(",", menuIdList);
            //TODO 设置菜单按钮
            return await Task.FromResult(false);
        }

    }
}
