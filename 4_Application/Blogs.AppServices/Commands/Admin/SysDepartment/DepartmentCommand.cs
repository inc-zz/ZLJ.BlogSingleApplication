using Blogs.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Commands.Admin.SysDepartment
{

    /// <summary>
    /// 部门维护
    /// </summary>
    public abstract class DepartmentCommand : Command
    {

        /// <summary>
        /// 部门Id
        /// </summary>
        public long Id { get; protected set; } 
        /// <summary>
        /// 上级部门
        /// </summary>
        public long ParentId { get; protected set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; protected set; }
        /// <summary>
        /// 简称
        /// </summary>
        public string Abbreviation { get; protected set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; protected set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; protected set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; protected set; }

    }
}
