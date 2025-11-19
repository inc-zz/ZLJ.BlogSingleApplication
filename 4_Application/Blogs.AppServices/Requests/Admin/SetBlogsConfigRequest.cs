using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Requests.Admin
{
    public class SetBlogsConfigRequest
    {

        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string? Url { get; set; }
        public string? Tags { get; set; }
        public string? BusType { get; set; }
        public string? Content { get; set; } 

    }
}
