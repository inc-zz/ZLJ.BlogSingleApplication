using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Requests.Admin
{

    /// <summary>
    /// 修改角色
    /// </summary>
    public class UpdateSysRoleParam:AddSysRoleRequest
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public long Id { get; set; }
    }
}
