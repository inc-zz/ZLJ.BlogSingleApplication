using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Core.Entity.Blogs
{
    /// <summary>
    /// 文章标签
    /// </summary>
    [SugarTable("blogs_tag")]
    public class DbBlogsTag : BaseEntity
    {
        /// <summary>
        /// 标签名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 标签连接
        /// </summary>
        public string LinkUrl { get; private set; }
        /// <summary>
        /// 标签颜色
        /// </summary>
        public string Color { get; private set; } 
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; private set; }
        /// <summary>
        /// 使用次数
        /// </summary>
        public int UsageCount { get; private set; } 
    }
}
