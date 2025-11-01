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
    /// 获取菜单按钮列表
    /// </summary>
    public class GetMenuButtonListQuery: IRequest<ResultObject<List<AuthMenuButtonDto>>>
    {
        public string? MenuName { get; set; }

    }
}
