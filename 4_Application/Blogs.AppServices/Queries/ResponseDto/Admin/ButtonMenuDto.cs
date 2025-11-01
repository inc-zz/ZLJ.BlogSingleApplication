using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.ResponseDto.Admin
{
    /// <summary>
    /// 按钮关联菜单DTO
    /// </summary>
    public class ButtonMenuDto
    {
        public long MenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuPath { get; set; }
    }
}
