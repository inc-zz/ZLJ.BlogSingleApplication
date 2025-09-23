using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Core.Models
{
    /// <summary>
    /// 分页返回类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageResult<T> : ResultObject where T: class 
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public long count { get; set; } 
        /// <summary>
        /// 数据集
        /// </summary>
        public List<T> data { get; set; }
    }
}
