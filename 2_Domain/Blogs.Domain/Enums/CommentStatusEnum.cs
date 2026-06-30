using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Domain.Enums
{
    /// <summary>
    /// 评论状态
    /// </summary>
    public enum CommentStatusEnum
    {
        /// <summary>
        /// 待审核
        /// </summary>
        Pending = 0,
        /// <summary>
        /// 已通过
        /// </summary>
        Approved,
        /// <summary>
        /// 已拒绝
        /// </summary>
        Rejected2
    }
}
