using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Requests.Admin
{

    /// <summary>
    /// 更改用户状态
    /// </summary>
    public class UpdateUserStatusRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 用户状态
        /// </summary>
        public int Status { get; set; }
    }
}
