using Blogs.AppServices.Queries.ResponseDto.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Requests.Admin
{
    public class AuthRoleMenuRequest
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public long RoleId { get; set; } 
        /// <summary>
        /// 角色菜单
        /// </summary>
        public List<AuthMenuRequest> RoleMenus { get; set; } = new List<AuthMenuRequest>();

    } 
     
}
