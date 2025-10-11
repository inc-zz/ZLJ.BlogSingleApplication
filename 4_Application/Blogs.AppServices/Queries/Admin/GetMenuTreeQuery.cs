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
    /// 获取菜单树查询
    /// </summary>
    public class GetMenuTreeQuery : IRequest<ResultObject<List<SysMenuTreeDto>>>
    {
        public int? Status { get; set; }
        public int? MenuType { get; set; }
    }

}
