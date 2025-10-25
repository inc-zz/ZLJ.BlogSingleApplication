using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Blogs.User
{
   public class AppRefreshTokenCommand : IRequest<ResultObject>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public AppRefreshTokenCommand(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
