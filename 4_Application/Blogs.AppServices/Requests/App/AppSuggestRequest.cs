using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Requests.App
{
    /// <summary>
    /// 提交建议请求对象
    /// </summary>
    public class AppSuggestRequest
    {

        public string? UserName { get; set; }

        public string? Contact { get; set; }

        public string? Content { get; set; }

    }
}
