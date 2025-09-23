using Blogs.AppServices.Commands.Admin.SysDepartment;
using Blogs.Core.Entity.Admin;
using Blogs.Domain.Entity.Admin;
using Mapster;
using MediatR;

namespace Blogs.AppServices.CommandHandlers.Admin
{

    /// <summary>
    /// 
    /// </summary>
    public class DepartmentCommandHandler: CommandHandler,
       IRequestHandler<CreateDepartmentCommand, bool>,
        IRequestHandler<UpdateDepartmentCommand,bool>,
        IRequestHandler<DeleteDepartmentCommand, bool>
    {
        private readonly IMediatorHandler _mediatorHandler;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediatorHandler"></param>
        public DepartmentCommandHandler(IMediatorHandler mediatorHandler):base(mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }


        /// <summary>
        /// 创建部门
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> Handle(CreateDepartmentCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
                return Task.FromResult(false);
            }
            var entity = command.Adapt<SysDepartment>();
            //entity.CreateTime = DateTime.Now;
            //entity.CreateUser = CurrentUserContext.Instance.Account;
            //entity.CreateUserName = CurrentUserContext.Instance.TrueName;
            //entity.Status = (int)ApproveStatusEnum.Normal;
            var response =  DbContext.Insertable(entity).ExecuteCommand();
            if (response > 0)
                return  Task.FromResult(true);
            return Task.FromResult(false);
        }

        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> Handle(UpdateDepartmentCommand command,CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
                return Task.FromResult(false);
            }

            var entity = command.Adapt<SysDepartment>();
            //entity.UpdateTime = DateTime.Now;
            //entity.UpdateUser = CurrentUserContext.Instance.Account;
            //entity.UpdateUserName = CurrentUserContext.Instance.TrueName;
            var response = DbContext.Updateable(entity).ExecuteCommand();
            if (response > 0)
                return Task.FromResult(true);
            return Task.FromResult(false);
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> Handle(DeleteDepartmentCommand command,CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
                return Task.FromResult(false);
            }
            var entity = DbContext.Queryable<SysDepartment>().Single(x=>x.Id == command.Id);
            //if(entity == null)
            //{
            //    _mediatorHandler.RaiseEvent(new DomainNotification("deleteDepartment", "删除失败"));
            //    return Task.FromResult(false);
            //}
            //var existsUser = DbContext.Queryable<SysUser>().Any(x => x.DepartmentId == command.Id);
            //if (existsUser)
            //{
            //    _mediatorHandler.RaiseEvent(new DomainNotification("deleteDepartment", "部门下存在用户，无法删除"));
            //    return Task.FromResult(false);
            //}

            //var existsRole = DbContext.Queryable<SysRole>().Any(x => x.DepartmentId == command.Id);
            //if (existsRole)
            //{
            //    _mediatorHandler.RaiseEvent(new DomainNotification("deleteDepartment", "部门下存在角色，无法删除"));
            //    return Task.FromResult(false);
            //}

            var deleteResult = DbContext.Deleteable<SysDepartment>().Where(x => x.Id == command.Id).ExecuteCommand();
            if (deleteResult > 0)
                return Task.FromResult(true);
            return Task.FromResult(false);
        }

    }
}
