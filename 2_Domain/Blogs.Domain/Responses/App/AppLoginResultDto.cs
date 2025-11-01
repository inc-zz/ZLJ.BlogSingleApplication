using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Infrastructure.Responses.App
{
    public class AppLoginResultDto
    {
        public AppLoginUserDto UserInfo { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? ExpiresIn { get; set; }
        public string TokenType { get; set; } = "Bearer";
    }
}
