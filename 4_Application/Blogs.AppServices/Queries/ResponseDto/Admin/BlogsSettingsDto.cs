using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.ResponseDto.Admin
{
    public class BlogsSettingsDto
    {
        public long Id { get; set; }
        public string? Title { get; private set; }
        public string? Summary { get; private set; }
        public string? Url { get; private set; }
        public string? Tags { get; private set; }
        public string? BusType { get; private set; }
        public string? Content { get; private set; }
        public int Status { get; private set; }
    }
}
