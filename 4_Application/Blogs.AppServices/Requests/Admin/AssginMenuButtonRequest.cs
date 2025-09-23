using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Requests.Admin
{

    /// <summary>
    /// 分配菜单按钮
    /// </summary>
    public class AssginMenuButtonRequest
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        public long MenuId { get; set; }
        /// <summary>
        /// 菜单按钮    
        /// </summary>
        public string Buttons { get; set; }
    }
}
