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
        public string Name { set; get; } 
        /// <summary>
        ///  菜单名称标识
        /// </summary>
        public string Code { set; get; }
        /// <summary>
        ///  菜单Url
        /// </summary>
        public string Url { set; get; }
        /// <summary>
        ///  菜单图标Class
        /// </summary>
        public string Icon { set; get; }
        /// <summary>
        ///  菜单排序
        /// </summary>
        public int Sort { set; get; } 
        /// <summary>
        /// 菜单功能按钮 多个按钮英文逗号分割
        /// </summary>
        public string Buttons { get; set; }
    } 

}
