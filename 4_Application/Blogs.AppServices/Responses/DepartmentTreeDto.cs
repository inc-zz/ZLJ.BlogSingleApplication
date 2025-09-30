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
        public string DepartmentName { get; set; } = string.Empty;
        public string DepartmentCode { get; set; } = string.Empty;
        public long? ParentId { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public List<DepartmentTreeDto> Children { get; set; } = new List<DepartmentTreeDto>();
    }
}
