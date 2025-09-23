using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Common.Models
{
    /// <summary>
    /// 用户认证模型
    /// </summary>
    public class UserTokenModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Jti { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime NotBefore { get; set; }
        public DateTime Expiration { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; } 

    }
}
