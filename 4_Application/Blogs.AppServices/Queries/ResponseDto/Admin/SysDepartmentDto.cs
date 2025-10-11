using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Queries.ResponseDto.Admin
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
        public string? Name { set; get; }
        ///<summary>
        ///  上级部门名称
        ///</summary>
        public string? ParentName { set; get; }
        ///<summary>
        ///  描述
        ///</summary>
        public string? Description { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string? CreatedBy { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? ModifiedAt { get;  set; }
        /// <summary>
        /// 最后修改人
        /// </summary>
        public string? ModifiedBy { get;  set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get;  set; }

        public List<SysDepartmentDto> Children { get; set; }

    }
}
