using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Queries.ResponseDto.Admin
{

    /// <summary>
    /// 菜单权限Dto
    /// </summary>
    public class RoleMenuPermissionDto
    {
        public long MenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuUrl { get; set; }
        public long ParentId { get; set; }
        public bool HasPermission { get; set; }
        public List<string> ButtonPermissions { get; set; } = new List<string>();
    }

}
