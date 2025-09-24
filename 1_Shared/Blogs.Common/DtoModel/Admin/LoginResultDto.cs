using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Core.DtoModel.Admin
{
    /// <summary>
    /// 登录结果
    /// </summary>
    public class LoginResultDto
    {
        public AdminUserDto UserInfo { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? ExpiresIn { get; set; }
        public string TokenType { get; set; } = "Bearer";
    }


}
