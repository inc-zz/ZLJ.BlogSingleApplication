using Blogs.AppServices.Queries.ResponseDto.App;
using Blogs.Core.Models;
using Blogs.Domain.Entity.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.App
{
    public class GetArticleCommentsQuery : PagedRequest, IRequest<ResultObject<List<BlogsCommentDto>>>
    {

        public long ArticleId { get; set; }

    }
}
