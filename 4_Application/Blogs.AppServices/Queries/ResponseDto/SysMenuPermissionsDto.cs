using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Core.DtoModel.ResponseDto
{

    /// <summary>
    /// 菜单权限Dto
    /// </summary>
    public class SysMenuPermissionsDto
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        public long MenuId { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }
        /// <summary>
        /// 子集菜单和按钮
        /// </summary>
        public List<SysMenuPermissionsDto> Childrens { get; set; }
        /// <summary>
        /// 菜单功能按钮
        /// </summary>
        public List<MenuButtonDto> Buttons { get; set; }
    }


}
