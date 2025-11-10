using Blogs.AppServices.Queries.ResponseDto.Admin;
using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Requests.Admin
{
     public class GetButtonDetailQuery : IRequest<ResultObject<ButtonDetailDto>>
    {
        public long Id { get; set; }

        public GetButtonDetailQuery(long id)
        {
            Id = id;
        }
    }
}
