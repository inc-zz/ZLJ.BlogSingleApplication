using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Core.DtoModel.RequestDto
{

    /// <summary>
    /// 排序字段扩展
    /// </summary>
    public class SortFilterRequest
    {
        /// <summary>
        /// 排序方式
        /// </summary>
        public DbOrderEnum SortOrder { get; set; } = DbOrderEnum.Asc;
        /// <summary>
        /// 排序字段
        /// </summary>
        public string? SortColumn { get; set; }

    }
}
