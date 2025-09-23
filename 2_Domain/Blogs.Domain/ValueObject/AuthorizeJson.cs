using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Domain.ValueObject
{

    /// <summary>
    /// 权限格式
    /// </summary>
    public class AuthorizeJson
    {
        /// <summary>
        /// 
        /// </summary>
        public long TenantId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsSuperAdmin { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public long RoleId { get; set; }
        /// <summary>
        /// 菜单
        /// </summary>
        public List<MenuObject> MenuInfos { get; set; }

    }
    
}
