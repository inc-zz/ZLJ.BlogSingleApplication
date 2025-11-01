using Blogs.AppServices.Queries.ResponseDto.Admin;
using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.Admin
{
    public class GetButtonListQuery : IRequest<PagedResult<SysButtonDto>>
    {
        public string Keyword { get; set; }
        public string ButtonType { get; set; }
        public string Position { get; set; }
    }
}
