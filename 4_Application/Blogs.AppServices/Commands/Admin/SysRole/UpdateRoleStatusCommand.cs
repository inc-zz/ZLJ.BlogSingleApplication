using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Commands.Admin.SysRole
{

    /// <summary>
    /// 修改角色状态（启用/禁用）
    /// </summary>
    public class UpdateRoleStatusCommand:RoleCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="status"></param>
        public UpdateRoleStatusCommand(long roleId,int status)
        {
            Id = roleId;
            Status = status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
