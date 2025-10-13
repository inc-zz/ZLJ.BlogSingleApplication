using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blogs.Domain;

namespace Blogs.AppServices.Commands.Blogs.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BlogsSettingCommand : Command
    {

        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string? Url { get; set; }
        public string? Tags { get; set; }
        public string? Content { get; set; }

        public string? BusType { get; set; }    
        public int Status { get; set; }


    }
}

