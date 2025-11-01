using Blogs.AppServices.Queries.ResponseDto.Admin;
using Blogs.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Admin.AuthManager
{
    /// <summary>
    /// 权限管理基础命令参数  
    /// </summary>
    public abstract class AuthCommand : Command
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public long RoleId { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string? RoleName { get; set; }
        /// <summary>
        /// 角色菜单
        /// </summary>
        public List<RoleMenuDto> RoleMenu { get; set; } = new List<RoleMenuDto>();

    }
}
