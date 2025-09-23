using Blogs.Core.DtoModel.Admin;
using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.Admin
{
    public class GetUserListQuery : IRequest<PagedResult<AdminUserDto>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SearchTerm { get; set; }
        public bool? IsActive { get; set; }
        public long? RoleId { get; set; }
        public string SortBy { get; set; }
        public bool SortDescending { get; set; }
    }
}
