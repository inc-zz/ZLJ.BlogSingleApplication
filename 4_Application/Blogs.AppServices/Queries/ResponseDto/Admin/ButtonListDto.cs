using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.ResponseDto.Admin
{
    /// <summary>
    /// 按钮列表DTO
    /// </summary>
    public class ButtonListDto
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string ButtonType { get; set; }
        public string Position { get; set; } 
        public int Status { get; set; }
        public string? StatusName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } 
    }
}
