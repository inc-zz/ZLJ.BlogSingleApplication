using Blogs.AppServices.Commands.Admin.SysRole;
using Blogs.Core.Entity.Admin;
using Blogs.Core.Enums;
using Blogs.Domain.Entity.Admin;

namespace Blogs.AppServices.CommandHandlers.Admin
{

    /// <summary>
    /// 角色模块相关命令操作
    /// </summary>
    public class RoleCommandHandler : CommandHandler,
        IRequestHandler<CreateRoleCommand, bool>,
        IRequestHandler<UpdateRoleCommand, bool>,
        IRequestHandler<DeleteRoleCommand, bool>,
        IRequestHandler<UpdateRoleStatusCommand, bool>

    {

        private readonly IMediatorHandler _mediatorHandler;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediatorHandler"></param>
        public RoleCommandHandler(IMediatorHandler mediatorHandler) : base(mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
                return await Task.FromResult(false);
            }
            var entity = command.Adapt<SysRole>();
            entity.MarkAsCreated(CurrentUser.Instance.UserInfo.UserName);
             
            entity.Status = (int)ApproveStatusEnum.Normal;

            var result = await DbContext.Insertable(entity).ExecuteCommandAsync();
            if (result > 0)
                return await Task.FromResult(true);
            return await Task.FromResult(false);

        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(UpdateRoleCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);

                return await Task.FromResult(false);
            }
            var entity = command.Adapt<SysRole>();
            entity.MarkAsModified(CurrentUser.Instance.UserInfo.UserName);

            var result = await DbContext.Updateable(entity).ExecuteCommandAsync();
            if (result > 0)
                return await Task.FromResult(true);
            return await Task.FromResult(false);

        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
                return await Task.FromResult(false);
            }
            var result = await DbContext.Deleteable<SysRole>().Where(x => x.Id == command.Id).ExecuteCommandAsync();
            if (result > 0)
                return await Task.FromResult(true);
            return await Task.FromResult(false);
        }

        /// <summary>
        /// 启用/禁用角色
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(UpdateRoleStatusCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
                return await Task.FromResult(false);
            }
            var result = await DbContext.Updateable<SysRole>().SetColumns(it => new SysRole { Status = command.Status })
                .Where(x => x.Id == command.Id).ExecuteCommandAsync();

            if (result > 0)
                return await Task.FromResult(true);
            return await Task.FromResult(false);
        }
    }
}
