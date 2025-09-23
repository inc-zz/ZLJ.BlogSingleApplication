using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Domain.ValueObject
{

    /// <summary>
    /// 菜单按钮
    /// </summary>
    public class MenuButton
    {
        /// <summary>
        /// 按钮名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 按钮Code
        /// </summary>
        public string? Code { get; set; }
            
    }
}
