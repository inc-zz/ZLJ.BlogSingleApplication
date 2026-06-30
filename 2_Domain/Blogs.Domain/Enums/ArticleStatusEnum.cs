using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Domain.Enums
{
    /// <summary>
    /// 文章状态枚举
    /// </summary>
    public enum ArticleStatusEnum
    {
        /// <summary>
        /// 已撤销
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 已发布
        /// </summary>
        Published,
        /// <summary>
        /// 草稿
        /// </summary>
        Draft   
    }
}
