using Blogs.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Commands.Admin.SysRole
{
    /// <summary>
    /// 角色信息
    /// </summary>
    public abstract class RoleCommand : Command
    {
        /// <summary>
        /// 角色
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 租户Id
        /// </summary>
        public long TenantId { get; set; }
        ///<summary>
        /// 角色组Id
        /// </summary>
        public long ParentId { get; set; }
        ///<summary>
        ///  角色名称
        ///</summary>
        public string Name { set; get; }
        ///<summary>
        ///  部门Id
        ///</summary>
        public long DepartmentId { set; get; }
        ///<summary>
        ///  部门名称
        ///</summary>
        public string DepartmentName { set; get; }
        ///<summary>
        ///  角色编号
        ///</summary>
        public string Code { set; get; }
        ///<summary>
        ///  是否超管员
        ///</summary>
        public int IsSystem { set; get; }
        ///<summary>
        ///  排序
        ///</summary>
        public int Sort { set; get; }
        ///<summary>
        ///  描述
        ///</summary>
        public string Summary { set; get; }
        /// <summary>
        /// 菜单
        /// </summary>
        public long MenuId { get; set; }
        /// <summary>
        /// 菜单Code
        /// </summary>
        public string MenuCode { get; set; }
        /// <summary>
        /// 菜单按钮
        /// </summary>
        public string ButtonCodes { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

    }
}
