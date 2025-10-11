using Blogs.AppServices.Queries.ResponseDto.Admin;
using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.Admin
{
    /// <summary>
    /// 获取菜单列表查询
    /// </summary>
    public class GetMenuListQuery : IRequest<PagedResult<SysMenuDto>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? MenuName { get; set; }
        public long? ParentId { get; set; }
    }
}
