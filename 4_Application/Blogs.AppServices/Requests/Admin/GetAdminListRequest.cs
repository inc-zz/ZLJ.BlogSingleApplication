using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Requests.Admin
{
    public class GetAdminListRequest: PageParam
    {

        public long Role { get; set; }

        public bool Status { get; set; }
    }
}
