using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Blogs.Domain
{
    /// <summary>
    /// Jwt用户信息
    /// </summary>
    public class JwtUserModel
    {
        /// <summary>
        /// 用户ID (主题标识)
        /// </summary>
        public string? UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// 用户角色
        /// </summary>
        public string? Roles { get; set; }
        /// <summary>
        /// 平台
        /// </summary>
        public string? Platform { get; set; }
        ///<summary>
        ///  描述
        ///</summary>
        public string? Description { set; get; }


    }
}
