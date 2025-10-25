using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Requests.App
{
    public class UpdateAppUserRequest
    {
        public long Id { get; set; }
        public string Remark { get; set; }
        public string? Email { set; get; }
        public string? PhoneNumber { set; get; }
    }
}
