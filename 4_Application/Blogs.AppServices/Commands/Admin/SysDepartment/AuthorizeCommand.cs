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
        /// <summary>
        /// 菜单
        /// </summary>
        public List<AuthMenuDto> AuthMenu { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<MenuButtonDto> MenuButtons { get; set; }




    }
    public class AuthMenuDto
    {
        public long MenuId { get; set; }

        public string MenuName { get; set; }
    }

    public class MenuButtonDto
    {
        public long MenuId { get; set; }

        public string[] Actions { get; set; }
    }
}
