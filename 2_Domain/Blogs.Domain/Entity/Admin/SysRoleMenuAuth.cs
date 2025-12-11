using System;

namespace Blogs.Domain.Entity.Admin
{
    /// <summary>
    /// 角色菜单授权表
    /// </summary>  
    [SugarTable("sys_rolemenu_auth")]
    public class SysRoleMenuAuth : BaseEntity
    {
        public SysRoleMenuAuth(long id,long menuid, string buttons)
        {
            this.Id = id;
            this.MenuId = menuid;
            this.ButtonPermissions = buttons;
        }

        public SysRoleMenuAuth()
        {
        }

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
        public string? ButtonPermissions { set; get; }
    }
}
