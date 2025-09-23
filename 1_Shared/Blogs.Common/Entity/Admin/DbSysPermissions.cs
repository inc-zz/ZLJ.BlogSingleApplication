using System;
using SqlSugar;

namespace Blogs.Core.Entity.Admin
{
    /// <summary>
    /// 角色按钮授权表
    /// </summary> 
    [SugarTable("sys_permissions")]
    public class DbSysPermissions : BaseEntity
    {
        ///<summary>
        ///  角色Id
        ///</summary>
        public long RoleId { set; get; }
        ///<summary>
        ///  菜单Id
        ///</summary>
        public long MenuId { set; get; }
        ///<summary>
        ///  菜单授权按钮
        ///</summary>
        public string? ButtonCodes { set; get; }

    }
}
