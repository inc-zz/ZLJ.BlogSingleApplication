using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blogs.AppServices.Requests.App
{

    /// <summary>
    /// 注册APP用户
    /// </summary>
    public class AddAppUserRequest
    {
        ///<summary>
        ///  登录账号
        ///</summary>
        public string UserName { set; get; }
        ///<summary>
        ///  登录密码
        ///</summary>
        public string Password { set; get; }

        public string? Email { set; get; }


    }
}
