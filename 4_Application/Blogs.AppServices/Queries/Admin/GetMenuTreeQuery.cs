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
        /// <summary>
        /// 状态：1启用，0禁用  
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 菜单类型:1目录，2:地址，3：外部链接
        /// </summary>
        public int? MenuType { get; set; }
    }

}
