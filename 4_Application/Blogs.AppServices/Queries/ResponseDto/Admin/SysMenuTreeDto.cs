using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.ResponseDto.Admin
{
    public class SysMenuTreeDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Icon { get; set; }
        public List<SysMenuTreeDto> Children { get; set; }
 
    }
}
