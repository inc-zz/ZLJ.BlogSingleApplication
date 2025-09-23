using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Domain.ValueObject
{

    /// <summary>
    /// 菜单和菜单按钮信息
    /// </summary>
    public class UserRoleMenu
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="button"></param>
        public UserRoleMenu(long menuId, MenuButton button)
        {
            this.MenuId = menuId;
            this.MenuButtons = button;
        }

        /// <summary>
        /// 菜单编码
        /// </summary>
        public long MenuId { get; protected set; }
        /// <summary>
        /// 菜单权限按钮
        /// </summary>
        public MenuButton MenuButtons { get; protected set; }
    }
}
