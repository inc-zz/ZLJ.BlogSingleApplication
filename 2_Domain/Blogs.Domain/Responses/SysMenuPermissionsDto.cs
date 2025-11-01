using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Infrastructure.Responses
{
    public class SysMenuPermissionsDto
    {
        public int MenuId { get; set; }

        public string MenuName { get; set; }

        public List<SysMenuButtonsDto> ButtonCodes { get; set; }

    }
}
