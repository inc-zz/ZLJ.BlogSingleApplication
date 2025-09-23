using SqlSugar;
using System;
namespace Blogs.Core.Entity.Admin
{
    /// <summary>
    /// 部门表实体
    /// </summary>
    [SugarTable("sys_department")]
    public class DbSysDepartment : BaseEntity
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
        /// 是否删除
        /// </summary>
        public int IsDelete { get; set; } = 0;

    }
}
