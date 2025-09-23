using System;

namespace Blogs.Domain.Entity.Admin
{
    /// <summary>
    /// 用户组
    /// </summary>  
    [SugarTable("sys_user_group_relation")]
    public class SysUserGroupRelation
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public long GroupId { get; set; }

    }
}
