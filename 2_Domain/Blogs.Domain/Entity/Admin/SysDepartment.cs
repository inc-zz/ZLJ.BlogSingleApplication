using System;
namespace Blogs.Domain.Entity.Admin
{
    /// <summary>
    /// 部门表实体
    /// </summary>
    [SugarTable("sys_department")]
    public class SysDepartment : BaseEntity
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 父级ID
        /// </summary>
        public long ParentId { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        public string? Abbreviation { get; set; }   
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<SysDepartment> Children { get; set; }

    }
}
