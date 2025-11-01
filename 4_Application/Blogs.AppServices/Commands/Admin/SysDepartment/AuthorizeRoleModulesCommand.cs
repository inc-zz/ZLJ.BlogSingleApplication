using Blogs.AppServices.Requests.Admin;
using Blogs.AppServices.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Admin.SysDepartment
{
    /// <summary>
    /// 角色菜单授权命令
    /// </summary>
    public class AuthorizeRoleModulesCommand : AuthorizeCommand
    {
       
        public AuthorizeRoleModulesCommand(AuthRoleToMenuButtonRequest request)
        {
            RoleId = request.RoleId;
            MenuPermissions = request.MenuPermissions;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
