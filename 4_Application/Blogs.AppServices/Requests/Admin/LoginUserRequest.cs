using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Requests.Admin
{
    /// <summary>
    /// 用户登录参数
    /// </summary>
    public class LoginUserRequest
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        public string? Account { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string? Password { get; set; }
    }
}
