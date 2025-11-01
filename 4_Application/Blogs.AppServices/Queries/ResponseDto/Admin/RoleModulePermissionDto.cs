using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.ResponseDto.Admin
{
    /// <summary>
    /// 角色模块权限数据传输对象
    /// </summary>
    public class RoleModulePermissionDto
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public long MenuId { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        public string MenuIcon { get; set; }

        /// <summary>
        /// 菜单类型（1：目录，2：菜单，3：按钮）
        /// </summary>
        public string MenuType { get; set; }

        /// <summary>
        /// 菜单URL
        /// </summary>
        public string MenuUrl { get; set; }

        /// <summary>
        /// 父级菜单ID
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 是否拥有权限
        /// </summary>
        public bool HasPermission { get; set; }

        /// <summary>
        /// 按钮权限列表
        /// </summary>
        public List<MenuButtonDto> ButtonPermissions { get; set; } = new List<MenuButtonDto>();

        /// <summary>
        /// 子菜单列表
        /// </summary>
        public List<RoleModulePermissionDto> Children { get; set; } = new List<RoleModulePermissionDto>();
    }
}
