using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Domain.ValueObject
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class MenuObject
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MenuCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MenuName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string[] Buttons { get; set; }
    }
}
