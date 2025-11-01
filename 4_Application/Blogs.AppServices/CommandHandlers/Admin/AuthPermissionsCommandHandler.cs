using Blogs.AppServices.Commands.Admin.AuthManager;
using Blogs.Domain.Notices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.CommandHandlers.Admin
{
    public class AuthPermissionsCommandHandler:CommandHandler,
        IRequestHandler<AuthRoleMenuCommand,bool>
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
            //需要从

            return true;
        }
    }
}
