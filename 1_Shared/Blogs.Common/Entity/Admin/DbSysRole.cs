using System;
using System.Xml.Linq;
using SqlSugar;

namespace Blogs.Core.Entity.Admin
{
    /// <summary>
    /// 角色表
    /// </summary> 
    [SugarTable("sys_role")]
    public class DbSysRole : BaseEntity
    {
        /// <summary>
        /// 是否管理员
        /// </summary>
        public int IsSystem { get; set; }
        ///<summary>
        ///  归属上级
        ///</summary>
        public long ParentId { set; get; }
        ///<summary>
        ///  角色名称
        ///</summary>
        public string? Name { set; get; }
        ///<summary>
        ///  角色编号
        ///</summary>
        public string? Code { set; get; }
        ///<summary>
        ///  备注信息
        ///</summary>
        public string? Remark { set; get; }
        ///<summary>
        ///  启用状态
        ///</summary>
        public int Status { set; get; }

    }
}
