using System;

namespace Blogs.Domain.Entity.Admin
{
    /// <summary>
    /// 用户组
    /// </summary>  
    [SugarTable("sys_user")]
    public class SysUserGroup: BaseEntity
    {
        ///<summary>
        ///  用户组名称
        ///</summary>
        public string? Name { set; get; }
        ///<summary>
        ///  用户组Code
        ///</summary>
        public string? Code { set; get; }
        ///<summary>
        ///  排序
        ///</summary>
        public int? Sort { set; get; }
        ///<summary>
        ///  状态
        ///</summary>
        public int IsDelete { set; get; } = 0;
        ///<summary>
        ///  备注
        ///</summary>
        public string? Remark { set; get; }

    }
}
