using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Requests.Admin
{

    /// <summary>
    /// 编辑管理员
    /// </summary>
    public class UpdateUserRequest 
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long Id { get; set; }
        ///<summary>
        ///  部门Id
        ///</summary>
        public string? DepartmentJson { set; get; }
        /// <summary>
        /// 用户角色
        /// </summary>
        public string? UserRoleJson { get; set; }
        public string? Description { set; get; }
        public string? Email { set; get; }
        public string? PhoneNumber { set; get; }
        public string? RealName { get; set; }
        public int Sex { get; set; } = 1;
    }
}
    