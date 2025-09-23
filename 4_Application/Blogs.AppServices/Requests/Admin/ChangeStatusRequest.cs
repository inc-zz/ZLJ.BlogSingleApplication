using Blogs.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Requests.Admin
{
    /// <summary>
    /// 修改状态请求参数
    /// </summary>
    public class ChangeStatusRequest
    {
        /// <summary>
        /// 业务Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 状态值
        /// </summary>
        public ApproveStatusEnum Status { get; set; }
    }
}
