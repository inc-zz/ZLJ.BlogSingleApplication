using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Core.DeoModel.Admin
{
    /// <summary>
    /// 登录结果
    /// </summary>
    public class LoginResultDto : ResultObject
    {
        public long UserId { get; set; }
        public string? UserName { get; set; }
        public string? RealName { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? ExpiresIn { get; set; }
        public string TokenType { get; set; } = "Bearer";
    }
}
