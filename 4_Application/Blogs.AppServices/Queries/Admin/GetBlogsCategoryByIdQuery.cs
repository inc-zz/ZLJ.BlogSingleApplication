using Blogs.AppServices.Queries.ResponseDto.Admin;
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
    public class GetBlogsCategoryByIdQuery : IRequest<BlogsCategoryDto>
    {
        public long Id { get; set; }

        public GetBlogsCategoryByIdQuery(long id)
        {
            Id = id;
        }
    }
}
