using System;
using SqlSugar;

namespace Blogs.Core.Entity.Admin
{
    /// <summary>
    /// 菜单表
    /// </summary> 
    [SugarTable("sys_menu")]
    public class DbSysMenu : BaseEntity
    {
        ///<summary>
        ///  菜单父级Id
        ///</summary>
        public long ParentId { set; get; } = 0;
        ///<summary>
        ///  菜单名称
        ///</summary>
        public string? Name { set; get; }
        ///<summary>
        ///  菜单名称标识
        ///</summary>
        public string? Code { set; get; }
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string? ICon { get; set; }
        /// <summary>
        /// 菜单类型：1目录，2:地址，3：外部链接
        /// </summary>
        public string? Type { get; set; }
        ///<summary>
        ///  菜单排序   
        ///</summary>
        public int Sort { set; get; }
        ///<summary>
        ///  是否可见：1是，0否
        ///</summary>
        public int Status { set; get; }
        ///<summary>
        ///  菜单按钮
        ///</summary>
        public string? Buttons { set; get; }

    }
}
