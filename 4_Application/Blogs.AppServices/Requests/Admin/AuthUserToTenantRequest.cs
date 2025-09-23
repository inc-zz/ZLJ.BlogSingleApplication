using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Requests.Admin
{

    /// <summary>
    /// 用户授权租户
    /// </summary>
    public class AuthUserToTenantRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 租户Id
        /// </summary>
        public long TenantId { get; set; }


    }
}
