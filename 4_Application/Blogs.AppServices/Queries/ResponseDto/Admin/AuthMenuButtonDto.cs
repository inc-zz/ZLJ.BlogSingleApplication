using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.ResponseDto.Admin
{
    /// <summary>
    /// 菜单按钮权限列表
    /// </summary>
    public class AuthMenuButtonDto
    {
        public long MenuId { get; set; }

        public string? MenuName { get; set; }

        public List<MenuButtonDto> MenuAuthButtons { get; set; } = new List<MenuButtonDto>();
        public List<AuthSysButtonDto> SysButtons { get; set; } = new List<AuthSysButtonDto>();
    }
}
