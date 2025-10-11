using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Responses
{
    public class DepartmentTreeDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Total { get; set; }
        public List<DepartmentTreeDto> Children { get; set; } = new List<DepartmentTreeDto>();
    }
}
