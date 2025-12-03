using Blogs.AppServices.Queries.ResponseDto.App;
using Blogs.Core.DtoModel.Admin;
using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.App
{
    public class GetAppUserListQuery : IRequest<PagedResult<BlogsUserDto>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int Status { get; set; } = 1;  
        public string Where { get; set; } = string.Empty;   
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }
    }
}
