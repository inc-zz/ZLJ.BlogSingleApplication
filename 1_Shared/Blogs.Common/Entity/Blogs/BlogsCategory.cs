using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Core.Entity.Blogs
{
    /// <summary>
    /// 文章分类
    /// </summary>
    [SugarTable("blogs_category")]
    public class DbBlogsCategory : BaseEntity
    {
        /// <summary>
        /// 分类名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 分类描述
        /// </summary>
        public string? Description { get; private set; }
        /// <summary>
        /// URL友好名称
        /// </summary>
        public string? Slug { get; private set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; private set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; private set; }
          
    }
}
