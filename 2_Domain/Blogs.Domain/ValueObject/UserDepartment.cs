using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Domain.ValueObject
{
    /// <summary>
    /// 
    /// </summary>
    public class UserDepartment
    {
        /// <summary>
        /// 
        /// </summary>
        public long DepartmentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long ParentDepartmentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DepartmentName { get; set; }

    }
}
