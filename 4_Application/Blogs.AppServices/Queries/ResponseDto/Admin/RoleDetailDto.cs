using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.ResponseDto.Admin
{
    /// <summary>
    /// 角色详情
    /// </summary>
    public class RoleDetailDto
    {
        public long Id { get; set; }
        public int IsSystem { get; set; }
        public long ParentId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }  
    }
}
