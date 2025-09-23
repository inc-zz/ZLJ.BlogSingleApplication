using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Commands.Admin.SysMenu
{

    /// <summary>
    /// 菜单按钮对象
    /// </summary>
    public class MenuButtonCommand
    {
        /// <summary>
        /// 菜单按钮对象
        /// </summary>
        /// <param name="buttonid"></param>
        /// <param name="name"></param>
        public MenuButtonCommand(string buttonid, string name)
        {
            ButtonId = buttonid;
            Name = name;
        }

        /// <summary>
        /// 按钮Code
        /// </summary>
        public string ButtonId { get; private set; }
        /// <summary>
        /// 按钮名称
        /// </summary>
        public string Name { get; private set; }

    }
}
