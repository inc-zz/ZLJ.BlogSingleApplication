using Blogs.AppServices.Responses;
using Blogs.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Admin.SysDepartment
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AuthorizeCommand : Command
    {
        /// <summary>
        /// 用户
        /// </summary>
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public List<RoleMenuAuthDto> MenuPermissions { get; set; } = new List<RoleMenuAuthDto>();
    } 
}
