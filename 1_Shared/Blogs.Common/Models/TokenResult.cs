
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Core.Models
{
    public class TokenResult
    {
        public bool Result { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
        public string Message { get; set; }

        public static TokenResult Success(string accessToken, string refreshToken, DateTime expiration)
        {
            return new TokenResult
            {
                Result = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expiration = expiration
            };
        }

        public static TokenResult Error(string message) => new TokenResult { Result = false, Message = message };
    }
}
