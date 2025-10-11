using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Queries.ResponseDto.Admin
{

    /// <summary>
    /// 菜单按钮
    /// </summary>
    public class SysMenuButtonsDto
    {
        /// <summary>
        /// 按钮Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 按钮名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 菜单按钮
        /// </summary>
        public List<MenuButtonDto> ButtonList { get; set; }
    }
}
