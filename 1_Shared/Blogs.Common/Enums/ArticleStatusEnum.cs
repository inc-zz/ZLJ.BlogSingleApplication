using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Core.Enums
{
    /// <summary>
    /// 文章发布状态
    /// </summary>
    public enum ArticleStatusEnum
    {
        Draft = 0,      // 草稿
        Published = 1,  // 已发布
        Archived = 2    // 已归档
    }
}
