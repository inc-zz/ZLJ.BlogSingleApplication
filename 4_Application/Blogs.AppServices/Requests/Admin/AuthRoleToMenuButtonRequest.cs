using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Requests.Admin
{

    /// <summary>
    /// 角色授权菜单按钮
    /// </summary>
    public class AuthRoleToMenuButtonRequest
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public long RoleId { get; set; }
        /// <summary>
        /// 角色菜单
        /// </summary>
        public RoleMenuInfo RoleMenus { get; set; }


    }
    /// <summary>
    /// 角色菜单
    /// </summary>
    public class RoleMenuInfo
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        public long MenuId { get; set; }
        /// <summary>
        /// 菜单按钮
        /// </summary>
        public List<string> ButtonIdList { get; set; }
    }
}
