using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Requests.Admin
{
    /// <summary>
    /// 新增菜单
    /// </summary>
    public class AddMenuRequest
    {
        /// <summary>
        ///  菜单父级Id
        /// </summary>
        public long ParentId { set; get; }
        /// <summary>
        ///  菜单名称
        /// </summary>
        public string? Name { set; get; }
        /// <summary>
        ///  菜单Url 
        /// </summary>
        public string? Url { set; get; }
        /// <summary>
        ///  菜单图标Class
        /// </summary>
        public string? Icon { set; get; }
        /// <summary>
        /// 菜单按钮
        /// </summary>
        public long[] Buttons { get; set; }
        /// <summary>
        /// 菜单类型：1目录，2:地址，3：外部链接
        /// </summary>
        public int Type { get; set; } = 2;
        /// <summary>
        ///  菜单排序
        /// </summary>
        public int Sort { set; get; }
    }

}
