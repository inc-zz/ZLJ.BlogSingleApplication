using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Responses
{
    /// <summary>
    /// 
    /// </summary>
    public class UserLoginDto
    {
        /// <summary>
     /// 请求令牌
     /// </summary>
        public string? Token { get; set; }
        /// <summary>
        /// 刷新令牌
        /// </summary>
        public string? RefreshToken { get; set; }
        /// <summary>
        /// 用户基本信息
        /// </summary>
        public UserBaseInfoDto UserInfo { get; set; }

        
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

    }
}
