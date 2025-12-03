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
    public class UpdateAppUserRequest
    {
        public long Id { get; set; }
        public string? Remark { get; set; }
        public string? Email { set; get; }
        public string? PhoneNumber { set; get; }
        public string? Avatar { get; set; }
    }
}
