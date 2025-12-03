using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Requests.Admin
{
    /// <summary>
    /// 管理员重置App用户密码请求参数
    /// </summary>
    public class ResetAppUserPasswordRequest
    {
        public long Id { get; set; }

        public string Password { get; set; }

    }
}
