using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Requests.Admin
{

    /// <summary>
    /// 修改部门
    /// </summary>
    public class UpdateDepartmentRequest
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        ///<summary>
        ///  父节点
        ///</summary>
        public long ParentId { set; get; }
        ///<summary>
        ///  部门名称
        ///</summary>
        public string Name { set; get; }
        ///<summary>
        ///  上级部门名称
        ///</summary>
        public string ParentName { set; get; }
        ///<summary>
        ///  部门简称
        ///</summary>
        public string Abbreviation { set; get; }
        ///<summary>
        ///  部门类型 0部门组，1部门
        ///</summary>
        public int Layer { set; get; }
        ///<summary>
        ///  排序
        ///</summary>
        public int Sort { set; get; }
        ///<summary>
        ///  描述
        ///</summary>
        public string Summary { set; get; }
    }
}
