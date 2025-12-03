using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Queries.ResponseDto.Admin
{
    /// <summary>
    /// 查询菜单按钮集合返还
    /// </summary>
    public class RoleMenuDto
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        public long MenuId { get; set; }
        /// <summary>
        /// 上级菜单
        /// </summary>
        public long ParentId { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string? MenuName { get; set; }
        /// <summary>
        /// 是否拥有权限
        /// </summary>
        public bool HasPermission { get; set; }
        /// <summary>
        /// 菜单按钮
        /// </summary>
        public List<MenuButtonDto> Buttons { get; set; } = new List<MenuButtonDto>();
    }
}
