using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Core.DtoModel.ResponseDto
{

    /// <summary>
    /// 
    /// </summary>
    public class SysDepartmentDto
    {

        ///<summary>
        ///  自增Id
        ///</summary>
        public long Id { set; get; }
        ///<summary>
        ///  组织名称
        ///</summary>
        public string Name { set; get; }
        ///<summary>
        ///  上级部门名称
        ///</summary>
        public string ParentName { set; get; }
        ///<summary>
        ///  简称
        ///</summary>
        public string Abbreviation { set; get; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        ///<summary>
        /// 审核状态
        ///</summary>
        public int Status { set; get; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public string StatusName { get; set; }
        ///<summary>
        ///  描述
        ///</summary>
        public string Summary { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreateUserName { get; set; }
        ///<summary>
        ///  修改时间
        ///</summary>
        public DateTime? UpdateTime { set; get; }
        ///<summary>
        ///  修改人名称
        ///</summary>
        public string UpdateUserName { set; get; }

    }
}
