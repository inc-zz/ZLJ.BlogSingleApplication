using Blogs.AppServices.Queries.ResponseDto.Admin;
using Blogs.Core.DtoModel.Admin;
using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.Admin
{
    /// <summary>
    /// 
    /// </summary>
    public class GetDepartmentListQuery : IRequest<PagedResult<SysDepartmentDto>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public long? DepId { get; set; }
        public string Where { get; set; }
    }
}
