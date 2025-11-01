using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Domain.ValueObject
{
    /// <summary>
    /// 用户部门信息
    /// </summary>
    public class UserDepartment
    {
        /// <summary>
        /// 部门Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; set; }

    }
}
