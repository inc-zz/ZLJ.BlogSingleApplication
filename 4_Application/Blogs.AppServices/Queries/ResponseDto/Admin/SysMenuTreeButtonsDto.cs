using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.ResponseDto.Admin
{
    /// <summary>
    /// 
    /// </summary>
    public class SysMenuTreeButtonsDto
    {
        public long MenuId { get; set; }
        public long ParentId { get; set; }   
        public string? Name { get; set; }
        public string? Icon { get; set; }
        public bool HasPermissions { get; set; }
        public List<MenuButtonDto> MenuButtons { get; set; } = [];
        public List<SysMenuTreeButtonsDto> Children { get; set; } = [];


    }
}
