using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Commands.Admin.SysRole
{

    /// <summary>
    /// 用户角色对象
    /// </summary>
    public class RoleIdsCommand
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleIds"></param>
        public RoleIdsCommand(string roleIds)
        {
            RoleIds = roleIds;
        }

        /// <summary>
        /// 
        /// </summary>
        public string RoleIds { get; private set; }



    }
}
