using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Domain.Enums
{
    public enum CommentStatusEnum
    {
        Pending = 0,    // 待审核
        Approved = 1,   // 已通过
        Rejected = 2    // 已拒绝
    }
}
