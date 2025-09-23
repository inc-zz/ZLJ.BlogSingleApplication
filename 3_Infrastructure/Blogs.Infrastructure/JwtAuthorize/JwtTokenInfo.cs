using Blogs.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Infrastructure.JwtAuthorize
{
    /// <summary>
    /// JWT令牌接口
    /// </summary>
    public class JwtTokenInfo
    {
        public string Subject { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public DateTime? Expires { get; set; }
        public Dictionary<string, string> Claims { get; set; }
        public string Header { get; set; }
        public string SignatureAlgorithm { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
