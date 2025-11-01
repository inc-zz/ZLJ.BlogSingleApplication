using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        ///  排序
        ///</summary>
        public int Sort { set; get; }
        ///<summary>
        ///  描述
        ///</summary>
        public string? Description { set; get; }
        ///<summary>
        ///  简称
        ///</summary>
        public string? Abbreviation { set; get; }
    }
}
