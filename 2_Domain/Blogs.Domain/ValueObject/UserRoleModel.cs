using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Domain.ValueObject
{

    /// <summary>
    /// 用户角色
    /// </summary>
    public class UserRoleModel
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public long RoleId { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string? RoleName { get; set; }

    }
}
