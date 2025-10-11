using Blogs.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.CommandHandlers.App
{
    public class AppCommandHandler : SqlSugarDbContext
    {


        private readonly IMediatorHandler _bus;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="bus"></param>
        public AppCommandHandler(IMediatorHandler bus)
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
