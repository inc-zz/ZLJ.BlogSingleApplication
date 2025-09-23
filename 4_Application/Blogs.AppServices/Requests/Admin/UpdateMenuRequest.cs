using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Requests.Admin
{
    /// <summary>
    /// 新增菜单
    /// </summary>
    public class UpdateMenuRequest : AddMenuRequest
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        public long Id { get; set; }
    }

}
