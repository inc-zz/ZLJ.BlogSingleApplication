using Blogs.Core.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Infrastructure.Constant
{
    [ConstantName(TableName = "分类模块")]
    public sealed class CategoryBusType
    {
        /// <summary>
        /// 类型：
        /// </summary>
        [ConstantName(ColumnName = "文章类型")]
        public const string ArticleType = "1";
        /// <summary>
        /// 领域：后端，前端，移动，运维，其他
        /// </summary>
        [ConstantName(ColumnName = "分类领域")]
        public const string ArticleDomain = "2";
    }
}
