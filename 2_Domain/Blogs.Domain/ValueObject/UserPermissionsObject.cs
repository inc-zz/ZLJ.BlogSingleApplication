using Blogs.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Domain.Commands
{

    /// <summary>
    /// 用户权限模型
    /// </summary>
    public class UserPermissionsObject 
    {
      
        /// <summary>
        /// 租户Id
        /// </summary>
        public long TenantId { get;  set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public long RoleId { get;  set; }
        /// <summary>
        /// 是否超管
        /// </summary>
        public long MenuId { get;  set; } 
 
    }
  
}
