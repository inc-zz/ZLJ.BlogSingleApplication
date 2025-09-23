using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Core.DtoModel
{

    /// <summary>
    /// 树形结构数据返回
    /// </summary>
    public class TreeNodeDto
    {
        /// <summary>
        /// 数值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 显示文本
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 子集
        /// </summary>
        public List<TreeNodeDto> Children { get; set; }
    }
}
