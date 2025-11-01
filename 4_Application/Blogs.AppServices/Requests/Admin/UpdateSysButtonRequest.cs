using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Requests.Admin
{
    public class UpdateSysButtonRequest: CreateSysButtonRequest
    {
        public long Id { get; set; }
    }
}
