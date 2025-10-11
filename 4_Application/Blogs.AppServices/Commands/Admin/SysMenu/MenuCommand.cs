using Blogs.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Commands.Admin.SysMenu
{

    /// <summary>
    /// 菜单对象
    /// </summary>
    public abstract class MenuCommand : Command
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; protected set; }
        /// <summary>
        /// 租户Id
        /// </summary>
        public long TenantId { get; protected set; }
        /// <summary>
        /// 父级Id
        /// </summary>
        public long ParentId { get; protected set; } 
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; protected set; }
        /// <summary>
        /// 菜单类型 1菜单，2页面，3外部链接
        /// </summary>
        public string Type { get; protected set; }
        /// <summary>
        /// 菜单Url
        /// </summary>
        public string Url { get; protected set; }
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string Icon { get; protected set; }
        /// <summary>
        /// 菜单状态
        /// </summary>
        public int Status { get; protected set; }
        /// <summary>
        /// 菜单按钮
        /// </summary>
        public List<MenuButtonCommand> Buttons { get; protected set; }

    }
}
