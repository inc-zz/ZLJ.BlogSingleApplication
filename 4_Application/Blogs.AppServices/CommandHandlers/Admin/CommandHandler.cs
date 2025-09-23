using System;
using System.Collections.Generic;
using System.Text;
using Blogs.Domain;
using MediatR;

namespace Blogs.AppServices.CommandHandlers.Admin
{
    /// <summary>
    /// 事件模型 
    /// </summary>
    public abstract class CommandHandler : SqlSugarDbContext
    {

        private readonly IMediatorHandler _bus;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="bus"></param>
        public CommandHandler(IMediatorHandler bus)
        {
            _bus = bus;
        }

        /// <summary>
        /// 将领域命令中的验证错误信息收集
        /// </summary>
        /// <param name="command"></param>
        protected void NotifyValidationErrors(Command command)
        {
            List<string> errorInfo = new List<string>();
            foreach (var error in command.ValidationResult.Errors)
            {
                _bus.RaiseEvent(new DomainNotification("domainNotice", error.ErrorMessage));
            }
        }
    }
}
