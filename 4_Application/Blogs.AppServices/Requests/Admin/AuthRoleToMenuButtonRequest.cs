using Blogs.AppServices.Responses;
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
         
        public List<RoleMenuAuthDto> MenuPermissions { get; set;} = new List<RoleMenuAuthDto>();

    } 
}
