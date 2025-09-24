using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Admin.SysUser
{

    /// <summary>
    /// 刷新令牌
    /// </summary>
    public class RefreshTokenCommand : IRequest<ResultObject>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public RefreshTokenCommand(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
