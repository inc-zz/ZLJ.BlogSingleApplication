﻿using System;

namespace Blogs.Domain.Entity.Admin
{

    /// <summary>
    /// 用户角色表
    /// </summary>

    [SugarTable("sys_user_role_relation")]
    public class SysUserRoleRelation 
    {
        public long Id { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public long RoleId { get; set; }

    }
}
