using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Requests.Admin
{
    /// <summary>
    /// 创建按钮请求参数
    /// </summary>
    public class CreateSysButtonRequest
    {
        /// <summary>
        /// 按钮名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 按钮Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 按钮位置：toolbar, row, both
        /// </summary>
        public string Positoin { get; set; } = "toolbar";

        public string? Description { get; set; }

        public string? Icon { get; set; }

        public int Sort { get; set; } = 0;

    }
}
