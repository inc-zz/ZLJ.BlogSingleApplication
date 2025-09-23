using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Requests.Admin
{

    /// <summary>
    /// 设置菜单按钮
    /// </summary>
    public class PutMenuButtonRequest
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        public long MenuId { get; set; }
        /// <summary>
        /// 按钮Id集合
        /// </summary>
        public List<string> ButtonsIdList { get; set; }

    }
}
