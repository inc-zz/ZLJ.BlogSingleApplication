using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Requests.Admin
{
    public class UpdateStatusRequest
    {

        public long Id { get; set; }

        public int Status { get; set; }
    }
}
