using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Responses
{
    /// <summary>
    /// 用户基本信息
    /// </summary>
    public class UserBaseInfoDto
    {

        public long UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }   
    }
}
