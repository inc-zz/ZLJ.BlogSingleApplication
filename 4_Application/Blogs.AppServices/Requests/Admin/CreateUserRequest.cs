using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Requests.Admin
{
    /// <summary>
    /// 创建App用户请求参数
    /// </summary>
    public class CreateUserRequest
    {
        public string? Account { get; set; }
        public string? Password { get; set; }
        public string? RealName { get; set; }
        public string? Remark { get; set; }
        public string? Email { set; get; }
        public string? PhoneNumber { set; get; }
    }
}
