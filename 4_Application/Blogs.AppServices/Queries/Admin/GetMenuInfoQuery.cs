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
    /// 获取菜单详情查询
    /// </summary>
    public class GetMenuInfoQuery : IRequest<ResultObject<SysMenuDto>>
    {
        public long Id { get; set; }
    }
}
