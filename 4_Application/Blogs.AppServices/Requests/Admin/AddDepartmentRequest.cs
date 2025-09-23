using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Requests.Admin
{

    /// <summary>
    /// 新增部门
    /// </summary>
    public class AddDepartmentRequest
    {
        ///<summary>
        ///  父节点
        ///</summary>
        public long ParentId { set; get; }
        ///<summary>
        ///  部门名称
        ///</summary>
        public string Name { set; get; } 
        ///<summary>
        ///  部门简称
        ///</summary>
        public string Abbreviation { set; get; }
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
