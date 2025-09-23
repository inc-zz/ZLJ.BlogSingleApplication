using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Domain
{
    /// <summary>
    /// Redis存储用户信息
    /// </summary>
    public class CacheUserModel
    {
        public long UserId { get; set; }
        /// <summary>
        /// 用户信息Json数据
        /// </summary>
        public JwtUserModel? UserInfo { get; set; } 
        /// <summary>
        /// 登录时间
        /// </summary>
        public string? LoginDate { get; set; }
        /// <summary>
        /// 权限数据
        /// </summary>
        public string? AuthorizeJson { get; set; }
        /// <summary>
        /// 是否超管
        /// </summary>
        public bool IsSuperAdmin { get; set; }

    }
}
