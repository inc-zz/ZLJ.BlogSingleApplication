using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Requests.Admin
{

    /// <summary>
    /// 完善用户资料
    /// </summary>
    public class UpdateUserRequest
    {
        
        /// <summary>
        /// 用户Id
        /// </summary>
        public long Id { get; set; }
        ///<summary>
        ///  角色Id
        ///</summary>
        public string RoleIds { set; get; } 
        ///<summary>
        ///  部门Id
        ///</summary>
        public string DepartmentId { set; get; }  
        ///<summary>
        ///  真实姓名
        ///</summary>
        public string TrueName { set; get; } 
        ///<summary>
        ///  性别
        ///</summary>
        public int Sex { set; get; }
        ///<summary>
        ///  头像
        ///</summary>
        public string HeadPic { set; get; }
        ///<summary>
        ///  手机号码
        ///</summary>
        public string Mobile { set; get; }
        ///<summary>
        ///  邮箱
        ///</summary>
        public string Email { set; get; }
        ///<summary>
        ///  描述
        ///</summary>
        public string Summary { set; get; } 

    }
}
