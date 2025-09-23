using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Core.Models
{
    /// <summary>
    /// Token返回格式
    /// </summary>
    public class ResultToken : ResultObject
    {
        /// <summary>
        /// 请求令牌
        /// </summary>
        public string? Token { get; set; }
        /// <summary>
        /// 刷新令牌
        /// </summary>
        public string? RefreshToken { get; set; }

    }
}
