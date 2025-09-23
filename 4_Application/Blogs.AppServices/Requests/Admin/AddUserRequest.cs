using System;
using System.Collections.Generic;
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
        public string Account { set; get; }
        ///<summary>
        ///  真实姓名
        ///</summary>
        public string TrueName { set; get; }
        ///<summary>
        ///  部门Id
        ///</summary>
        public long DepartmentId { set; get; } 
        ///<summary>
        ///  登录密码
        ///</summary>
        public string Password { set; get; }

    }
}
