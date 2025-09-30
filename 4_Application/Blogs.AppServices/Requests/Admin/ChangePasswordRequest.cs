using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Requests.Admin
{
    public class ChangePasswordRequest
    {
        public long UserId { get; set; }

        public string Password { get; set; }

        public string OldPassword { get; set; }

    }
}
