using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Core.Models
{

    /// <summary>
    /// 分页查询通用查询类
    /// </summary>
    public class PageParam
    {
        /// <summary>
        /// 当前页
        /// </summary> 
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页总条数
        /// </summary>
        public int PageSize { get; set; } = 30;
        /// <summary>
        /// 查询条件
        /// </summary>
        public string? Where { get; set; }

    }
}
