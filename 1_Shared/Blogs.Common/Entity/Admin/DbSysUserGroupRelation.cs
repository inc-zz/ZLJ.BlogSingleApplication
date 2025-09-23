using System;
using SqlSugar;

namespace Blogs.Core.Entity.Admin
{
    /// <summary>
    /// 用户组
    /// </summary> 
    [SugarTable("sys_user_group_relation")]
    public class DbSysUserGroupRelation
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }

        public long UserId { get; set; }

        public long GroupId { get; set; }

    }
}
