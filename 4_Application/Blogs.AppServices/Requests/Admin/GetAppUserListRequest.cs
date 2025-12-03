using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Requests.Admin
{
    /// <summary>
    /// 后台管理-获取App用户列表请求参数
    /// </summary>
    public class GetAppUserListRequest : PageParam
    {
        public int Status { get; set; } = 1;
    }
}
