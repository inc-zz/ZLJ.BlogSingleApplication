using Blogs.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.WebSite
{
    public abstract class WebSiteCommand : Command
    {

        public string? UserName { get; set; }

        public string? Contact { get; set; }

        public string? Content { get; set; }

    }
}
