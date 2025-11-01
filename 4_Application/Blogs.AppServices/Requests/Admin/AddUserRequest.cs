using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blogs.AppServices.Requests.Admin
{

    /// <summary>
    /// 新增用户
    /// </summary>
    public class AddUserRequest
    {
        ///<summary>
        ///  登录账号
        ///</summary>
        public string UserName { set; get; }
        ///<summary>
        ///  登录密码
        ///</summary>
        public string? Password { set; get; }
        ///<summary>
        ///  部门Id
        ///</summary>
        public string? DepartmentJson{ set; get; }
        /// <summary>
        /// 用户角色
        /// </summary>
        public string? UserRoleJson { get; set; }
        public string? Description { set; get; }
        public string? Email { set; get; }
        public string? PhoneNumber { set; get; }
        public string? RealName { get; set; }

        public int Sex { get; set; }

    }
}
