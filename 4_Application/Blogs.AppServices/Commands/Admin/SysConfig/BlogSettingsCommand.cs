using Blogs.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Admin.SysConfig
{
    public abstract class BlogSettingsCommand : Command
    {
        public long Id { get; set; }
        public string? Title { get;  set; }
        public string? Summary { get;  set; }
        public string? Url { get;  set; }
        public string? Tags { get;  set; }
        public string? BusType { get;  set; }
        public string? Content { get;  set; }
        public int Status { get;  set; }

    }
}
